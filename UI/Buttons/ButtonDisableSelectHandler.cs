using Bounder.Framework;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static ButtonPlaySFX;

[RequireComponent(typeof(Button))]
public class ButtonDisableSelectHandler : MonoBehaviour
{
    public enum ButtonDirection
    {
        ANY,
        UP,
        LEFT,
        RIGHT,
        DOWN
    }

    [SerializeField]
    private ButtonDirection nextButtonDir;

    private Button button;
    private bool selected;
    private bool interactable;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (button != null)
        {
            if (button.interactable)
            {
                interactable = true;
                selected = EventSystem.current.currentSelectedGameObject == gameObject;
            }
            else
            {
                if (interactable && selected)
                {
                    Selectable neighbor = null;
                    switch (nextButtonDir)
                    {
                        case ButtonDirection.ANY:
                            {
                                neighbor = button.FindSelectableOnUp();
                                if (neighbor == null)
                                {
                                    neighbor = button.FindSelectableOnLeft();
                                }
                                if (neighbor == null)
                                {
                                    neighbor = button.FindSelectableOnRight();
                                }
                                if (neighbor == null)
                                {
                                    neighbor = button.FindSelectableOnDown();
                                }
                            }
                            break;
                        case ButtonDirection.UP:
                            {
                                neighbor = button.FindSelectableOnUp();
                            }
                            break;
                        case ButtonDirection.LEFT:
                            {
                                neighbor = button.FindSelectableOnLeft();
                            }
                            break;
                        case ButtonDirection.RIGHT:
                            {
                                neighbor = button.FindSelectableOnRight();
                            }
                            break;
                        case ButtonDirection.DOWN:
                            {
                                neighbor = button.FindSelectableOnDown();
                            }
                            break;

                        default:
                            break;
                    }

                    if (neighbor != null)
                    {
                        neighbor.Select();
                    }
                }
                interactable = false;
                selected = false;
            }
        }
    }


}
