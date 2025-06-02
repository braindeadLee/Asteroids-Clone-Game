using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public struct AsteroidType
{
    public float scale;
    public int score;

    public AsteroidType(float scale, int score)
    {
        this.scale = scale;
        this.score = score;
    }
}

public struct ChilderoidTraits
{
    public float child1accelleration;
    public float child2accelleration;
    public Quaternion child1rotation;
    public Quaternion child2rotation;

    public ChilderoidTraits(float child1accelleration,  float child2accelleration, Quaternion child1rotation, Quaternion child2rotation)
    {
        this.child1accelleration = child1accelleration;
        this.child2accelleration = child2accelleration;
        this.child1rotation = child1rotation;
        this.child2rotation = child2rotation;
    }
}

public class EnemyManager : MonoBehaviour
{
    public GameObject asteroid;

    public AsteroidType[] AsteroidTypes = new AsteroidType[]
    {
        new AsteroidType(1f, 50),
        new AsteroidType(0.75f, 80),
        new AsteroidType(0.5f, 100)
    };

    public ChilderoidTraits[] childTraits = new ChilderoidTraits[]
    {
        new ChilderoidTraits(0.7f, 0.7f, Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, 45)),
        new ChilderoidTraits(0.7f, 0.7f, Quaternion.Euler(0, 0, -45), Quaternion.Euler(0, 0, 45)),
        new ChilderoidTraits(0.5f, 0.5f, Quaternion.Euler(0, 0, -60), Quaternion.Euler(0, 0, 60)),
        new ChilderoidTraits(0.5f, 0.6f, Quaternion.Euler(0, 0, -135), Quaternion.Euler(0, 0, 45)),
        new ChilderoidTraits(0.3f, 0.8f, Quaternion.Euler(0, 0, -15), Quaternion.Euler(0, 0, 15)),
        new ChilderoidTraits(0f, 0.7f, Quaternion.Euler(0, 0, -30), Quaternion.Euler(0,0,25))
    };
    //Calculator for creating asteroid spawn coordinates 
    private BoundsCalculators AsteroidCoordinatesCalculator;

    private Vector2 _BoundsMax;

    private BoxCollider2D _SafezoneCollider;

    private float _accelerationMultiplier = 1f;

    private float _screenWidth;
    private float _screenHeight;

    public static event UnityAction AsteroidDeath;

    public static event UnityAction<int> AsteroidScore;

    //Used in OnAsteroidDeath to prevent repeating variable decs
    int TraitIndex = 0;
    Quaternion rotationValue = Quaternion.Euler(0f, 0f, 0f);
    float accelSpeed = 0f;

    private void Awake()
        {

        AsteroidCoordinatesCalculator = new BoundsCalculators();

        //Get height and width of camera
        CameraUtility.CalculateDimensions();
        _screenWidth = (CameraUtility.width / 2) - 1;
        _screenHeight = (CameraUtility.height / 2) - 1;

        //Box's Bounds: Minimum Bounds = (-4.17, -1.70), Maximum Bounds = (4.10, 1.80)
        //Debug.Log("Minimum Bounds = " + _BoundsMin + ", Maximum Bounds = " + _BoundsMax);

        //Height and Width of The Screen = Width = 17.77778, Length = 10
        //Debug.Log("Width = " + CameraUtility.width + ", Length = " + CameraUtility.height);
    }

    public void SetMultiplier(float multiplier) => _accelerationMultiplier = multiplier;

    public void SpawnAsteroids(int asteroidCount)
    {
        //Get safezone's max bounds.x & .y
        _SafezoneCollider = GetComponent<BoxCollider2D>();
        _BoundsMax = _SafezoneCollider.bounds.max;
        //Debug.Log("Maximum Bounds = " + _BoundsMax);


        for (int i = 0; i < asteroidCount; i++)
        {
            Vector3 asteroidPosition = AsteroidCoordinatesCalculator.SquareBoundsCoordinates(_BoundsMax.x, _BoundsMax.y, _screenWidth, _screenHeight);

            Quaternion _asteroidRotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));

            GameObject newAsteroid = Instantiate(asteroid, asteroidPosition, _asteroidRotation);
        }
    }

    public void OnAsteroidDeath(float asteroidScale, Vector3 asteroidPosition, Quaternion asteroidRotation, float asteroidSpeed, int asteroidType)
    {

        AsteroidScore?.Invoke(AsteroidTypes[asteroidType].score);

        if (asteroidType < AsteroidTypes.Length - 1)
        {

            TraitIndex = Random.Range(0, childTraits.Length);

            //Debug.Log("Trait Chosen: child1rotation: " + childTraits[TraitIndex].child1rotation + ", child2rotation: " + childTraits[TraitIndex].child2rotation + ", child1accel: " + childTraits[TraitIndex].child1accelleration + ", child2accel: " + childTraits[TraitIndex].child2accelleration);

            rotationValue = childTraits[TraitIndex].child1rotation;
            accelSpeed = childTraits[TraitIndex].child1accelleration;

            GameObject childeroid1 = Instantiate(asteroid, asteroidPosition, asteroidRotation * rotationValue);
            //Assigns childeroid's type to the higher rank
            childeroid1.GetComponent<AsteroidPhysics>()._asteroidType = ++asteroidType;

            childeroid1.GetComponent<AsteroidPhysics>()._asteroidScale = AsteroidTypes[asteroidType].scale;

            //Increase childeroid's speed by 1, then multiply with _accelerationMultiplier
            childeroid1.GetComponent<AsteroidPhysics>()._asteroidSpeed += accelSpeed;
            childeroid1.GetComponent<AsteroidPhysics>()._asteroidSpeed *= _accelerationMultiplier;

            rotationValue = childTraits[TraitIndex].child2rotation;
            accelSpeed = childTraits[TraitIndex].child2accelleration;

            GameObject childeroid2 = Instantiate(asteroid, asteroidPosition, asteroidRotation * rotationValue);
            childeroid2.GetComponent<AsteroidPhysics>()._asteroidType = asteroidType;
            childeroid2.GetComponent<AsteroidPhysics>()._asteroidScale = AsteroidTypes[asteroidType].scale;
            childeroid2.GetComponent<AsteroidPhysics>()._asteroidSpeed += accelSpeed;
            childeroid2.GetComponent<AsteroidPhysics>()._asteroidSpeed *= _accelerationMultiplier;

            
        }
        else
        {
            AsteroidDeath?.Invoke();
        }
    }

    public void DestroyAsteroids()
    {
        GameObject[] currentAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        foreach (GameObject asteroid in currentAsteroids)
        {
            Destroy(asteroid);
        }
    }

    public bool AreThereAsteroids()
    {
        GameObject[] currentAsteroids = GameObject.FindGameObjectsWithTag("Asteroid");
        int asteroidCount = currentAsteroids.Length;
        if(asteroidCount == 1)
        {
            return false;
        }

        return true;
    }
}
