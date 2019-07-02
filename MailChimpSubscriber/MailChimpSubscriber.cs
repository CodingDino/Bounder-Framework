﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Net.Mail;


namespace Fiftytwo
{
    public class MailChimpSubscriber : MonoBehaviour
    {
        private const string UrlFormat = "https://{0}.api.mailchimp.com/3.0/lists/{1}/members";
        private const string DataFormat = "{{\"email_address\":\"{0}\", \"status\":\"subscribed\"}}";

        [SerializeField]
        private string _apiKey = "";
        [SerializeField]
        private string _listId = "";

        [SerializeField]
        private UnityEvent _subscribeSuccess = null;
        [SerializeField]
        private UnityEvent _subscribeError = null;


        public void Subscribe ()
        {
            var text = GetComponent<Text>();

            if( text == null )
            {
                Debug.LogError( "MailChimp — No UI Text found at this GameObject" );
                return;
            }
            else
            {
                Subscribe( text.text );
            }
        }

        public void Subscribe ( string email )
        {
            if( IsValidEmail( email ) )
            {
                var www = BuildWWW( email );

                if( www != null )
                {
                    StartCoroutine( SendToMailChimp( www ));
                }
                else
                {
                    _subscribeError.Invoke();
                }
            }
            else
            {
                Debug.Log( "MailChimp — Invalid email" );
                _subscribeError.Invoke();
            }
        }

        private bool IsValidEmail ( string email )
        {
            if( string.IsNullOrEmpty( email ) )
                return false;

            try
            {
                var mailAdress = new MailAddress( email );

                return mailAdress != null;
            }
            catch( FormatException )
            {
                return false;
            }
        }

        private IEnumerator SendToMailChimp ( WWW www )
        {
            yield return www;

            if( string.IsNullOrEmpty( www.error ) )
            {
                Debug.Log( "MailChimp — Subscribe success" );
                _subscribeSuccess.Invoke();
            }
            else
            {
                Debug.Log( "MailChimp — Subscribe error: " + www.error );
                _subscribeError.Invoke();
            }
        }

        private WWW BuildWWW ( string email )
        {
            var headers = new Dictionary<string,string>();
            headers.Add( "Authorization", "apikey " + _apiKey );

            var data = string.Format( DataFormat, email );
            var dataBytes = Encoding.ASCII.GetBytes( data );

            var splittedApiKey = _apiKey.Split( '-' );

            if( splittedApiKey.Length != 2 )
            {
                Debug.LogError( "MailChimp — Invalid API Key format" );
                return null;
            }

            var urlPrefix = splittedApiKey[1];

            var url = string.Format( UrlFormat, urlPrefix, _listId );
            var www = new WWW( url, dataBytes, headers );

            return www;
        }
    }
}
