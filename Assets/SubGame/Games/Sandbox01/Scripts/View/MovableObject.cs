using UnityEngine;
using System.Collections;
using UniKid.Core;

namespace UniKid.SubGame.Games.Sandbox01.View
{
	public sealed class MovableObject : CoreView
    {
        private TouchScript.Gestures.TapGesture _tapGesture;
        private RaycastHit _hit;

        protected override void Start()
        {
            base.Start();

            _tapGesture = GetComponent<TouchScript.Gestures.TapGesture>();
        }

        private void Update()
        {
            if (_tapGesture.State == TouchScript.Gestures.Gesture.GestureState.Changed)
            {

                var ray = Camera.main.ScreenPointToRay(_tapGesture.ScreenPosition);

                if (!Physics.Raycast(ray, out _hit)) return;

                Transform.localPosition = new Vector3(_hit.point.x, _hit.point.y, Transform.localPosition.z);
            }

        }

        private void OnCollisionEnter(Collision collision)
        {
            _tapGesture.enabled = false;
        }

        private void OnCollisionExit(Collision collision)
        {
            _tapGesture.enabled = true;
        }

        private void OnCollisionStay(Collision collision)
        {
            StartCoroutine(GetOutOfBorder(collision.contacts[0].normal));
        }

        private IEnumerator GetOutOfBorder(Vector3 normal)
        {
            gameObject.rigidbody.velocity = normal * 5;
            yield return new WaitForSeconds(0.2f);
            gameObject.rigidbody.velocity = Vector3.zero;
        }
    }
}