using UnityEngine;
using strange.extensions.mediation.impl;

namespace UniKid.Core
{
	public class CoreView: EventView
	{
		public Transform Transform { get; private set; }

		protected override void Start ()
		{
			base.Start();
			Transform = transform;
		}
	}
}