using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Q9Core;

[CreateAssetMenu(fileName = "Q9InitialLibrary", menuName = "Q9InitialLibrary")]
public class Q9InitialLibrary : ScriptableObject {

    public Q9Ship[] _ships;
    public Q9Module[] _modules;
    public Q9Item[] _items;
}
