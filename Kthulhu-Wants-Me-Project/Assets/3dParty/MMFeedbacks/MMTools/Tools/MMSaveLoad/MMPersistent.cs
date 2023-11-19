using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace MoreMountains.Tools
{
	/// <summary>
	/// A persistent class that can save the essential parts of an object :
	/// its transform data (position, rotation, scale) and its active state
	/// This inherits from MMPersistentBase and implements the IMMPersistent interface
	/// It's a good example of how to implement the interface's OnSave and OnLoad methods
	/// </summary>
	public class MMPersistent : MMPersistentBase
	{
		
		[Header("Properties")]
		/// whether or not to save this object's position
		[Tooltip("whether or not to save this object's position")]
		public bool SavePosition = true;
		/// whether or not to save this object's rotation
		[Tooltip("whether or not to save this object's rotation")]
		public bool SaveLocalRotation = true;
		/// whether or not to save this object's scale
		[Tooltip("whether or not to save this object's scale")]
		public bool SaveLocalScale = true;
		/// whether or not to save this object's active state
		[Tooltip("whether or not to save this object's active state")]
		public bool SaveActiveState = true;
		
		/// <summary>
		/// A struct used to store and serialize the data we want to save
		/// </summary>
		[Serializable]
		public struct Data 
		{
			public Vector3 Position;
			public Quaternion LocalRotation;
			public Vector3 LocalScale;
			public bool ActiveState;
		}

		/// <summary>
		/// On Save, we turn the object's transform data and active state to a Json string and return it to the MMPersistencyManager
		/// </summary>
		/// <returns></returns>
		public override string OnSave()
		{
			return JsonUtility.ToJson(new Data { Position = this.transform.position, 
													LocalRotation = this.transform.localRotation, 
													LocalScale = this.transform.localScale, 
													ActiveState = this.gameObject.activeSelf });
		}

		/// <summary>
		/// On load, we read the saved json data and apply it to our object's properties
		/// </summary>
		/// <param name="data"></param>
		public override void OnLoad(string data)
		{
			if (SavePosition)
			{
				this.transform.position = JsonUtility.FromJson<Data>(data).Position;
			}

			if (SaveLocalRotation)
			{
				this.transform.localRotation = JsonUtility.FromJson<Data>(data).LocalRotation;	
			}

			if (SaveLocalScale)
			{
				this.transform.localScale = JsonUtility.FromJson<Data>(data).LocalScale;	
			}

			if (SaveActiveState)
			{
				this.gameObject.SetActive(JsonUtility.FromJson<Data>(data).ActiveState);	
			}
		}
	}	
}

