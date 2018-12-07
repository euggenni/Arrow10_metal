using UnityEngine;
using System.Collections;

public class UIVideo : MonoBehaviour
{
	public bool Cycled;

	private MovieTexture _movie;
	private bool isPlaying = false;

	void Set(MovieTexture movie)
	{
		if (movie != null)
		{
			_movie = movie;
			PlayMovie();
		}
	}

	void PlayMovie()
	{
		if (_movie != null)
		{
			_movie.Play();

			isPlaying = true;
			enabled = true;
		}
	}
	
	void StopMovie()
	{
		if (_movie != null)
		{
			_movie.Stop();
			isPlaying = false;
			enabled = false;
		}
	}

	// Update is called once per frame
	void Update ()
	{
		if (_movie != null)
		{
			if (isPlaying)
			{
				if (!_movie.isPlaying)
				{
					StopMovie();

					if (Cycled)
						PlayMovie();
				}
			}
		}
		else enabled = false;
	}
}
