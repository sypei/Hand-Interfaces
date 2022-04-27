using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace com.guinealion.animatedBook {
	[ExecuteInEditMode]
	public class LightweightBookHelper : MonoBehaviour {

		private Material m_material;

		[Header("Control")]
		[SerializeField] [Range(0,1)] private float _open = 0;
		[SerializeField] [Range(-1, 1)] private float _orientation = 0;
		[SerializeField] private int _pagePairs = 6;
		[SerializeField] private float _progress = 0;

		/// <summary>
		/// Makes the book updates every frame, this is useful for animations where the property fields will not be called
		/// </summary>
		public bool updateEveryFrame = false;

		[Header("Components")]
		[SerializeField] private SkinnedMeshRenderer m_meshRenderer = null;
		[SerializeField] private Transform m_frontCover = null;
		[SerializeField] private Transform m_backCover = null;
		[SerializeField] private Transform m_boneRoot = null;

		const string Material_OpenAmmount = "_Open";
		const string Material_Pages = "_Pages";
		const string Material_Progress = "_Progress";

		private bool loadedData = false;
		private Coroutine pageAnimation;
		private Coroutine openAnimation;

        private static Vector3 closedOffset = new Vector3(0, 0, -0.0535f);
        private static Vector3 closedOrientationOffset = new Vector3(-0.0535f, 0, 0.0535f * 2);

		/// <summary>
		/// Determines how much the book is Opened
		/// where 0 stands for closed and 1 stands for Open
		/// </summary>
		public float OpenAmmount {
			get {
				return _open;
			}
			set {
				_open = value;

				m_backCover.localEulerAngles = new Vector3(0, 0,- 90 * value);
				m_frontCover.localEulerAngles = new Vector3(0, 0,90 * value);

                m_boneRoot.localEulerAngles = new Vector3(0, (90 * Orientation) * (1 - Mathf.Abs(value)), -90);

                Vector3 orientationOffset = closedOrientationOffset * Mathf.Abs(Orientation);
                orientationOffset.x *= Mathf.Sign(Orientation);
                 
                m_boneRoot.localPosition = (closedOffset + orientationOffset) * (1 - value);

                m_material.SetFloat(Material_OpenAmmount, value * value);
			}
		}

		/// <summary>
		/// Informs the ammount of pages the book has
		/// </summary>
		public int PageAmmount {
			get {
				return _pagePairs;
			}
			set {
				_pagePairs = value;
				m_material.SetFloat(Material_Pages, value - 1);
				Progress = _progress;
			}
		}

		/// <summary>
		/// Informs the book progress over the pages:
		/// 0 - first page
		/// 1 - second page
		/// 1.35 - 35% lerp between second and third page
		/// </summary>
		public float Progress {
			get {
				return _progress;
			}
			set {
				_progress = Mathf.Clamp(value, 0, _pagePairs - 1);
				m_material.SetFloat(Material_Progress, _progress);
			}
		}

		/// <summary>
		/// Determines which direction the shader should open or close
		/// IMPORTANT: The shader only supports "half openned page flip" with orientation 0, otherwise, you should only change the progress with the book fully open!
		/// </summary>
		public float Orientation {
			get {
				return _orientation;
			}
			set {
				_orientation = Mathf.Clamp(value, -1, 1);
				OpenAmmount = _open;
			}
		}

		void Awake() {
			LoadData();
		}

		private void OnValidate() {
			LoadData();
			OpenAmmount = _open;
		}

		void LoadData() {
#if UNITY_EDITOR
			if(!loadedData || m_material == null)
				m_material = Application.isPlaying ? m_meshRenderer.material : m_meshRenderer.sharedMaterial;
#else
			if (loadedData) return;
			m_material = m_meshRenderer.material;
#endif
			PageAmmount = _pagePairs;

			loadedData = true;
		}

		void Update() {
			if (!updateEveryFrame) return;
			OpenAmmount = _open;
			PageAmmount = _pagePairs;
		}

		/// <summary>
		/// Advances to the next page
		/// </summary>
		public void NextPage(float duration = 0.45f){
			GoToPage(Mathf.FloorToInt(_progress) + 1, false, duration);
		}

		/// <summary>
		/// Returns to the previous page
		/// </summary>
		public void PrevPage(float duration = 0.45f) {
			GoToPage(Mathf.FloorToInt(_progress) - 1, false, duration);
		}

		/// <summary>
		/// Go to specific page
		/// </summary>
		/// <param name="page">The page to navigate</param>
		public void GoToPage(int page) {
			GoToPage(page, false, 0.45f);
		}

		/// <summary>
		/// Go to specific page
		/// </summary>
		/// <param name="page">The page to navigate</param>
		/// <param name="findClosest">Find closest page if requested page is out of range</param>
		/// <param name="duration">The page animation duration</param>
		public void GoToPage(int page, bool findClosest = true, float duration = 0.45f) {
			if(page < 0 || page >= PageAmmount) {
				if (findClosest) {
					page = page < 0 ? 0 : PageAmmount - 1;
				} else {
					Debug.LogWarningFormat("Trying to move to a page out of range ({0})", page);
					return;
				}
			}
			if(pageAnimation != null) {
				StopCoroutine(pageAnimation);
			}
			pageAnimation = StartCoroutine(DoChangeProgress(page, duration));
		}

		/// <summary>
		/// Opens the book
		/// </summary>
		public void Open(float duration = 0.5f) {
			if (openAnimation != null)
				StopCoroutine(openAnimation);
			openAnimation = StartCoroutine(DoChangeOpen(1, duration));
		}

		/// <summary>
		/// Closes the book
		/// </summary>
		public void Close(float duration = 0.5f) {
			if (openAnimation != null)
				StopCoroutine(openAnimation);
			if(pageAnimation != null) {
				StopCoroutine(pageAnimation);
				Progress = Mathf.Round(Progress);
			}
				
			openAnimation = StartCoroutine(DoChangeOpen(0, duration));
		}

		IEnumerator DoChangeProgress(int targetPage, float pageDuration) {
			do {
				Progress = Mathf.MoveTowards(Progress, targetPage, Time.deltaTime / pageDuration);
				yield return null;
			} while (Mathf.Abs(targetPage - Progress) > 0.01f);
			Progress = targetPage;
		}

		IEnumerator DoChangeOpen(int direction, float duration) {
			do {
				OpenAmmount = Mathf.MoveTowards(OpenAmmount, direction, Time.deltaTime / duration);
				yield return null;
			} while (Mathf.Abs(direction - OpenAmmount) > 0.01f);
			OpenAmmount = direction;
		}

	}
}