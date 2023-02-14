using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Tobii.Gaming.Examples.GazePointData
{
    public class EyeTracker : MonoBehaviour
    {
		[Range(3.0f, 15.0f), Tooltip("Number of gaze points in point cloud.")]
		public int PointCloudSize = 10;

		[Tooltip("Sprite to use for gaze points in the point cloud.")]
		public Sprite PointSprite;

		[Range(0.0f, 1.0f), Tooltip("Scale to draw the point sprites in the point cloud.")]
		public float PointScale = 0.1f;

		[Tooltip("Distance from screen to visualization plane in the World.")]
		public float VisualizationDistance = 10f;

		[Range(0.1f, 1.0f),
		 Tooltip(
			 "How heavy filtering to apply to gaze point bubble movements. 0.1f is most responsive, 1.0f is least responsive.")]
		public float FilterSmoothingFactor = 0.15f;

		private GazePoint _lastGazePoint = GazePoint.Invalid;



		
		// Members used for the gaze point cloud:
		private const float MaxVisibleDurationInSeconds = 0.5f;
		private GazePoint[] _gazePoints;
		private int _last;
		private GameObject[] _gazePointCloudSprites;

		// Members used for gaze bubble (filtered gaze visualization):
		private SpriteRenderer
			_gazeBubbleRenderer; // the gaze bubble sprite is attached to the GazePlotter game object itself

		private bool _useFilter = false;
		private bool _hasHistoricPoint;
		private Vector3 _historicPoint;

		public bool UseFilter
		{
			get { return _useFilter; }
			set { _useFilter = value; }
		}


		public bool showGP;
		public GameObject eyeTrackerPoint;
		public bool mouseController;
		public float speedMouse = 1;
		private bool activated;

		// Start is called before the first frame update
		void Start()
        {
			if (showGP || mouseController)
			{
				InitializeGazePointBuffer();
				InitializeGazePointCloudSprites();

				_last = PointCloudSize - 1;

				_gazeBubbleRenderer = GetComponent<SpriteRenderer>();

				eyeTrackerPoint.GetComponent<RectTransform>().position = Vector3.zero;
				activated = true;
			}
            else
            {
				eyeTrackerPoint.SetActive(false);
				activated = false;
			}
			
		}

        // Update is called once per frame
        void Update()
        {
			if (showGP == true || mouseController == true)
			{
				if (!activated)
				{
					eyeTrackerPoint.SetActive(true);
					eyeTrackerPoint.GetComponent<RectTransform>().position = Vector3.zero;
					activated = true;
				}
			}
			else
			{
				eyeTrackerPoint.SetActive(false);
				activated = false;
			}

			GazePoint gazePoint = TobiiAPI.GetGazePoint();
            if (gazePoint.IsValid)
            {
                Vector2 gazePosition = gazePoint.Screen;
                //yCoord.color = xCoord.color = Color.white;
                Vector2 roundedSampleInput =
                    new Vector2(Mathf.RoundToInt(gazePosition.x), Mathf.RoundToInt(gazePosition.y));
                //xCoord.text = "x (in px): " + roundedSampleInput.x;
                //yCoord.text = "y (in px): " + roundedSampleInput.y;

                //Debug.Log("x = " + roundedSampleInput.x + "     y = " + roundedSampleInput.y);

				if (showGP == true)
                {
					showGazePoint(gazePoint);
                }
            }
			else
            {
				//Debug.Log("Invalid gaze point !");
            }
			if (mouseController)
			{
				/*if (Input.GetAxis("Mouse X") != 0)
				{
					eyeTrackerPoint.GetComponent<RectTransform>().position += new Vector3(speedMouse * Input.GetAxis("Mouse X") * Time.deltaTime, 0, 0);
				}
				if (Input.GetAxis("Mouse Y") != 0)
				{
					eyeTrackerPoint.GetComponent<RectTransform>().position += new Vector3(0, speedMouse * Input.GetAxis("Mouse Y") * Time.deltaTime, 0);
				}*/
				eyeTrackerPoint.GetComponent<RectTransform>().position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, eyeTrackerPoint.GetComponent<RectTransform>().position.z);
			}
		}


		void showGazePoint(GazePoint gazePoint)
		{
			if (gazePoint.IsRecent()
				&& gazePoint.Timestamp > (_lastGazePoint.Timestamp + float.Epsilon))
			{
				UpdateGazePointCloud(gazePoint);
				_lastGazePoint = gazePoint;
				Vector2 gazePosition = gazePoint.Screen;
				//yCoord.color = xCoord.color = Color.white;
				Vector2 roundedSampleInput = new Vector2(Mathf.RoundToInt(gazePosition.x), Mathf.RoundToInt(gazePosition.y));
				eyeTrackerPoint.GetComponent<RectTransform>().position = new Vector3(roundedSampleInput.x, roundedSampleInput.y, eyeTrackerPoint.GetComponent<RectTransform>().position.z);

			}
			UpdateGazePointCloudVisibility();
		}

		private void InitializeGazePointBuffer()
		{
			_gazePoints = new GazePoint[PointCloudSize];
			for (int i = 0; i < PointCloudSize; i++)
			{
				_gazePoints[i] = GazePoint.Invalid;
			}
		}

		private void InitializeGazePointCloudSprites()
		{
			_gazePointCloudSprites = new GameObject[PointCloudSize];
			for (int i = 0; i < PointCloudSize; i++)
			{
				var pointCloudSprite = new GameObject("PointCloudSprite" + i);
				pointCloudSprite.layer = gameObject.layer;

				var spriteRenderer = pointCloudSprite.AddComponent<SpriteRenderer>();
				spriteRenderer.sprite = PointSprite;

				var cloudPointVisualizer = pointCloudSprite.AddComponent<CloudPointVisualizer>();
				cloudPointVisualizer.Scale = PointScale;

				pointCloudSprite.SetActive(false);
				_gazePointCloudSprites[i] = pointCloudSprite;
			}
		}

		public bool get_activated()
        {
			return activated;
        }

		private void UpdateGazePointCloudVisibility()
		{
			bool isPointCloudVisible = !UseFilter;

			for (int i = 0; i < PointCloudSize; i++)
			{
				if (IsNotTooOld(_gazePoints[i]))
				{
					_gazePointCloudSprites[i].SetActive(isPointCloudVisible);
				}
				else
				{
					_gazePointCloudSprites[i].SetActive(false);
				}
			}
		}

		private bool IsNotTooOld(GazePoint gazePoint)
		{
			return (Time.unscaledTime - gazePoint.Timestamp) < MaxVisibleDurationInSeconds;
		}


		private void UpdateGazePointCloud(GazePoint gazePoint)
		{
			_last = Next();
			_gazePoints[_last] = gazePoint;
			var cloudPointVisualizer = _gazePointCloudSprites[_last].GetComponent<CloudPointVisualizer>();
			Vector3 gazePointInWorld = ProjectToPlaneInWorld(gazePoint);
			cloudPointVisualizer.NewPosition(gazePoint.Timestamp, gazePointInWorld);
		}

		private int Next()
		{
			return ((_last + 1) % PointCloudSize);
		}

		private Vector3 ProjectToPlaneInWorld(GazePoint gazePoint)
		{
			Vector3 gazeOnScreen = gazePoint.Screen;
			gazeOnScreen += (transform.forward * VisualizationDistance);
			return Camera.main.ScreenToWorldPoint(gazeOnScreen);
		}

		private Vector3 Smoothify(Vector3 point)
		{
			if (!_hasHistoricPoint)
			{
				_historicPoint = point;
				_hasHistoricPoint = true;
			}

			var smoothedPoint = new Vector3(
				point.x * (1.0f - FilterSmoothingFactor) + _historicPoint.x * FilterSmoothingFactor,
				point.y * (1.0f - FilterSmoothingFactor) + _historicPoint.y * FilterSmoothingFactor,
				point.z * (1.0f - FilterSmoothingFactor) + _historicPoint.z * FilterSmoothingFactor);

			_historicPoint = smoothedPoint;

			return smoothedPoint;
		}
	}
}
