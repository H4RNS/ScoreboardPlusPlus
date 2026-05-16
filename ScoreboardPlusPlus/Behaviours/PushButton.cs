using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScoreboardPlusPlus.Behaviours
{
    public class PushButton : MonoBehaviour
    {
        private float _buttonDelay = 0.25f;
        private Color _originalColor;
        private Image _image;

        private List<string> _savedButtonInfo = [];
        private Action _onPushButtonPress;

        private float _lastPressTime;
        private int _buttonIndex = 1;
        private bool _isDynamic = false;

        public static PushButton CreateStatic(Transform _content, Action _action)
        {
            return SetupButton(_content, _action);
        }

        public static PushButton CreateDynamic(Transform _content, List<string> _buttonInfo, Action _action)
        {
            PushButton _button = SetupButton(_content, _action);
            _button._savedButtonInfo = _buttonInfo; _button._isDynamic = true;
            return _button;
        }

        private static PushButton SetupButton(Transform _obj, Action _action)
        {
            if (_obj.gameObject == null) return null;

            _obj.gameObject.SetLayer(UnityLayer.GorillaInteractable);

            PushButton _button = _obj.GetComponent<PushButton>() ?? _obj.AddComponent<PushButton>();
            _button._onPushButtonPress = _action;

            BoxCollider _collider = _obj.GetComponent<BoxCollider>() ?? _obj.AddComponent<BoxCollider>();
            _collider.isTrigger = true;

            _button._originalColor = _obj.GetComponent<Image>().color;
            _button._image = _obj.GetComponent<Image>();
            return _button;
        }

        private void OnTriggerEnter(Collider _collider)
        {
            if (Time.time < _lastPressTime + _buttonDelay) return;
            _lastPressTime = Time.time;

            if (!_collider.TryGetComponent(out GorillaTriggerColliderHandIndicator hand)) return;

            GorillaTagger.Instance.offlineVRRig.PlayHandTapLocal(67, hand.isLeftHand, 0.05f);
            GorillaTagger.Instance.StartVibration(hand.isLeftHand, GorillaTagger.Instance.tapHapticStrength / 2f, GorillaTagger.Instance.tapHapticDuration);

            if (_isDynamic && _savedButtonInfo.Count > 0)
            {
                transform.Find("Text").GetComponent<Text>().text = _savedButtonInfo[_buttonIndex];
                _buttonIndex = (_buttonIndex + 1) % _savedButtonInfo.Count;
            }

            StartCoroutine(ChangeButtonColour(0.2f));
            _onPushButtonPress.Invoke();
        }

        private IEnumerator ChangeButtonColour(float _delay)
        {
            PushButton _button = GetComponent<PushButton>();

            Color _darkerColour = _button._originalColor * 0.7f;
            _darkerColour.a = _button._originalColor.a;

            _button._image.color = _darkerColour;

            yield return new WaitForSeconds(_delay);

            _button._image.color = _button._originalColor;
        }
    }
}