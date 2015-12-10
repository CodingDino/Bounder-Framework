// ************************************************************************ 
// File Name:   JSON.cs 
// Purpose:    	JSON framework for use with Bounder Games data
// Project:		Framework
// Author:      Sarah Herzog  
// Copyright: 	2015 Bounder Games
// ************************************************************************ 


// ************************************************************************ 
// Imports 
// ************************************************************************
using UnityEngine; 
using MiniJSON;
using System;
using System.Collections;
using System.Collections.Generic;


// ************************************************************************ 
// Class: JSON 
// ************************************************************************ 
namespace BounderFramework { 

	public interface Archive {
		bool Load(JSON _JSON);
		JSON Save();
	}

	public class JSON : IEnumerable<JSON> {

		public bool HasValidData() { return valid && data != null; }

        private object __data = null;

        public object data
        {
            get
            {
                return __data;
            }
            set
            {
				if (value is Archive)
				{
					data = (value as Archive).Save().data;
					return;
				}

				if (value is JSON)
				{
					data = (value as JSON).data;
					return;
				}

				__data = value;
				if (parent != null && key != "ROOT")
				{
					int index = -1;
					if( int.TryParse(key, out index) )
						parent[index] = this;
					else
						parent[key] = this;
				}
            }
        }

		private bool __valid = true;
        public bool valid
        {
            get
            {
                return __valid;
            }
            set
            {
                __valid = value;
                if (parent != null)
                {
                    parent.valid = parent.valid && value;
                }
            }
        }


        public string key = "ROOT";
		public JSON parent = null;

		// ****************************************************************

		static public JSON ParseString(string _stringJSON)
		{
			JSON result = new JSON();
			result.data = Json.Deserialize(_stringJSON);
			result.valid = true;
			return result;
		}

		// ****************************************************************

		public override string ToString ()
		{
			if (valid)
				return Json.Serialize(data);
			else
			{
				Debug.LogError("Attempt to deserialize invalid JSON");
				return "{}";
			}
		}

		// ****************************************************************

		public JSON() {}
		public JSON(object _object,  JSON _parent = null, string _key = "ROOT", bool _valid = true)
		{
			data = _object;
			valid = _valid;
			key = _key;
			parent = _parent;
		}
		
		
		// ****************************************************************

		public string path {
			get {
				if (parent != null)
					return parent.path + "[" + key + "]";
				else return key;
			}
		}
		
		// ****************************************************************

		public JSON this[string _key]
		{
			get
            {
                if (data == null) // Create dictionary
                    data = new Dictionary<string, object>();

                if (valid && data != null)
				{
					Dictionary<string,object> N = data as Dictionary<string,object>;
					if (N != null && N.ContainsKey(_key))
						return new JSON(N[_key], this, _key);
                    else if (N != null && !N.ContainsKey(_key)) // auto create member
                    {
                        N[_key] = null;
                        return new JSON(null, this, _key, true); // valid, but empty.
                    }
				}
				JSON invalidJSON = new JSON(null, this, _key, false);
				Debug.LogError("JSON dictionary get access failed for object "+invalidJSON.path);
				return invalidJSON;
			}
			set {
                value.parent = this;
                value.key = _key;
                value.valid = value.valid && valid;

                if (data == null)
                    data = new Dictionary<string, object>();

                if (valid && data != null)
                {
                    Dictionary<string, object> N = data as Dictionary<string, object>;
                    if (N != null)
                    {
                        N[_key] = value.data;
                    }
                }
                else
                {
                    valid = false;
                    value.valid = false;
                    JSON invalidJSON = new JSON(null, this, _key, false);
                    Debug.LogError("JSON dictionary set access failed for object " + invalidJSON.path);
                }
            }
		}
		
		// ****************************************************************
		
		public JSON this[int _index]
		{
			get
            {
                if (data == null) // Create List
                    data = new List<object>();

                if (valid && data != null)
				{
					List<object> N = data as List<object>;
					
					if (N != null && _index < N.Count)
						return new JSON(N[_index], this, _index.ToString());
                    else if (N != null && _index >= N.Count) // auto create member(s) 
                    {
                        while (_index >= N.Count)
                        {
                            N.Add(null);
                        }
                        return new JSON(N[_index], this, _index.ToString());
                    }
                }
				JSON invalidJSON = new JSON(null, this, _index.ToString(), false);
				Debug.LogError("JSON array get access failed for object "+invalidJSON.path);
				return invalidJSON;
			}
			set {
					value.valid = value.valid && valid;
					value.key = _index.ToString();
					value.parent = this;

	                if (data == null)
	                    data = new List<object>();

	                if (valid && data != null)
	                {
	                    List<object> N = data as List<object>;

	                    if (N != null && _index < N.Count)
							N[_index] = value.data;
	                    else if (N != null && _index >= N.Count)
	                    {
	                        while (_index >= N.Count)
	                        {
	                            N.Add(null);
	                        }
							N[_index] = value.data;
	                    }
	                }
	                else
	                {
	                    valid = false;
						value.valid = false;
	                    JSON invalidJSON = new JSON(null, this, _index.ToString(), false);
	                    Debug.LogError("JSON array set access failed for object " + invalidJSON.path);
	                }
            }
		}
		
		// ****************************************************************

		public Archive GetArchive(Archive _default)
		{
			Archive result = _default;
			GetArchive(ref result);
			return result;
		}
		public bool GetArchive(ref Archive _result)
		{
			if (valid && data != null && _result != null)
			{
				if(_result.Load(this))
					return true;
			}
			//Debug.LogError("JSON archive access failed for object " + path);
			return false;
		}
		
		// ****************************************************************
		
		public T GetArchive<T>() where T : Archive, new()
		{
			T result = new T();
			if(GetArchive(ref result))
				return result;
			else
				return default(T);
		}
		public T GetArchive<T>(T _default) where T : Archive, new()
		{
			T result = _default;
			GetArchive<T>(ref result);
			return result;
		}
		public bool GetArchive<T>(ref T _result) where T : Archive, new()
		{
			if (_result == null)
				_result = new T();

			if (valid && data != null && _result != null)
			{
				if(_result.Load(this))
					return true;
				else
					Debug.LogError("JSON Archive parse failed for object at path " + path);
			}
			//Debug.LogError("JSON archive access failed for object " + path);
			return false;
		}

        // ****************************************************************

        public T GetObject<T>() where T : class
        {
            return GetObject(default(T));
        }
        public T GetObject<T>(T _default) where T : class
        {
            T result = _default;
            GetObject(ref result);
            return result;
        }
		public bool GetObject<T>(ref T _result) where T : class
        {
            T result = data as T;
            if (valid && result != null)
            {
				_result = result;
                return true;
            }
            //Debug.LogError("JSON generic access failed for object " + path);
			return false;
		}
		
        // ****************************************************************

        public int GetInt(int _default = 0)
        {
            int result = _default;
            Get(ref result);
            return result;
        }
        public bool Get(ref int _result)
        {
            if (valid && data != null)
            {
				return int.TryParse(data.ToString(), out _result);
            }
            //Debug.LogError("JSON Int access failed for object " + path);
            return false;
        }

        // ****************************************************************

        public float GetFloat(float _default = 0)
        {
            float result = _default;
            Get(ref result);
            return result;
        }
        public bool Get(ref float _result)
        {
            if (valid && data != null)
			{
				return float.TryParse(data.ToString(), out _result);
            }
            //Debug.LogError("JSON Float access failed for object " + path);
            return false;
        }

        // ****************************************************************

        public bool GetBool(bool _default = false)
        {
            bool result = _default;
            Get(ref result);
            return result;
        }
        public bool Get(ref bool _result)
        {
            if (valid && data != null)
			{
                return bool.TryParse(data.ToString(), out _result);
            }
            //Debug.LogError("JSON Bool access failed for object " + path);
            return false;
        }

        // ****************************************************************

        public string GetString(string _default = "")
        {
            string result = _default;
            Get(ref result);
            return result;
        }
        public bool Get(ref string _result)
        {
			if (valid && data != null && data is string)
            {
                _result = data as string;
                return true;
            }
            //Debug.LogError("JSON String access failed for object " + path);
            return false;
        }

        // ****************************************************************
        
        public T GetEnum<T>(T _default = default(T))
        {
            T result = _default;
            GetEnum(ref result);
            return result;
        }
        public bool GetEnum<T>(ref T _result)
        {
			if (valid && data != null && data as string != null)
            {
				string enumString = data as string;
				if (enumString != null &&  Enum.IsDefined(typeof(T), enumString))
				{
                	_result = (T)Enum.Parse(_result.GetType(), enumString);
					return true;
				}
			}
			//Debug.LogError("JSON Enum access failed for object " + path);
            return false;
        }

        // ****************************************************************

        public IEnumerator<JSON> GetEnumerator()
        {
            List<object> dataList = data as List<object>;
            Dictionary<string, object> dataDic = data as Dictionary<string, object>;
            if (valid && dataList != null)
            {
                for (int i = 0; i < dataList.Count; ++i)
                {
                    yield return new JSON(dataList[i], this, i.ToString());
                }
            }
            else if (valid && dataDic != null)
            {

                foreach (KeyValuePair<string, object> pair in dataDic)
                {
                    yield return new JSON(pair.Value, this, pair.Key);
                }
            }
            else
            {
                //Debug.LogError("JSON enumerator failed for object " + path);
            }

        }

        // Explicit interface implementation for nongeneric interface
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator(); // Just return the generic version
        }

    }

}
