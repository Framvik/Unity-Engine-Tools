using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Framvik.EditorTools;

public class ScriptableObjectUtilityTests
{
    [Test]
    public void ScriptableObjectUtilityGetAllInstancesTest()
    {
        Assert.IsTrue(ScriptableObjectUtility.GetAllInstances<ExistingScriptableObject>().Length >= 1);
        Assert.IsTrue(ScriptableObjectUtility.GetAllInstances<NonExistingScriptableObject>().Length <= 0);
    }
}
