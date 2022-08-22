# Saving-System-Extension

This extension will help you to save a bunch of complex data in a binary format and load them back.

## Setup

1. Copy the URL of this repository - https://github.com/Vijay2K/Saving-System-Extension.git

2. Open the unity editor and then go to Windows -> Package Manager.

3. Click on the plus icon at the cornor of the package manager window.

<img src="https://user-images.githubusercontent.com/64386924/185951623-d02f9046-e6eb-4590-8c3a-e62eeea933ef.png" width="300" height="100">

4. Then click the option "**Add package from git URL...**" and then paste the repository URL.

Then all the dependencies will be included in the project.

## How to use?

* Create a empty gameobject and add the component `SavingSystem`.

<img src="https://user-images.githubusercontent.com/64386924/185962742-cc378830-6c50-46a6-a342-201bada9b263.png" width="300" height="175">

* Need to include namespace for using the functions from saving system class

```yaml

using Extension.SavingSystem;

```

* Then you can call the function `Save(string fileName)` and `Load(string fileName)` from any class.

#### Example

```yaml

private const string fileName = "Data"

private void SaveData()
{
  SavingSystem.Instance.Save(fileName);
}

private void LoadData()
{
  SavingSystem.Instance.Load(fileName);
}

```

* Add the `SaveableEntity` to the gameobjects that you need to save properties / components of the gameobjects. Without adding `SaveableEntity` data won't be get saved for those objects.

<img src="https://user-images.githubusercontent.com/64386924/185966860-3fbe475e-a760-4998-9818-6b7e6c49b04b.png" width="300" height="175">

* This saveable entity will give some random unique id for those gameobjects.

<img src="https://user-images.githubusercontent.com/64386924/185967406-b5d9f2f2-c016-4a01-b247-03a30dfcfdcb.png" width="300" height="80">

* If we need to save the health then the Health class needs to inherit from `ISavable` interface and implement the methods of an interface `CaptureState()` and `RestoreState(object state)`.
  `CaptureState()` will save the data and `RestoreState(object state)` will restore/load the data. 

* We can save the any data by inheriting the `ISavable` interface. 

#### Example

```yaml

public class Health : MonoBehaviour, ISavable
{
    private float health = 150;

    public void TakeDamage(float damage)
    {
      health -= damage;
    }

    //This capture state will save the data
    public object CaptureState()
    {
      return health;
    }

    //This restore state will load the data
    public void RestoreState(object stateToRestore)
    {
      health = (float)stateToRestore;
    }
}

```

* We can't save the Vectors using this. For that we need to use `SerializableVector` because for saving in the binary format the class need to serializable.

#### Example

```yaml

public class Player : MonoBehaviour, ISavable
{
  public object CaptureState()
  {
    SerializableVector position = new Serializable(transform.position);
    return position;
  }
  
  public void RestoreState(object state)
  {
    Serializable position = (Serializable)state;
    transform.position = position.ToVector3();
  }
}

```

