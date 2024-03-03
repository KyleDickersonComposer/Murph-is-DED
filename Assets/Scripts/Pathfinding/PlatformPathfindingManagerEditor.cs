using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Game.Pathfinding
{
   [CustomEditor(typeof(PlatformPathfindingManager))]
    public class PlatformPathfindingManagerEditor : Editor
    {
       private SerializedProperty _heuristicTypeProp;
       private SerializedProperty _calculatorPatienceProp;
       private SerializedProperty _allNodesProp;
       private SerializedProperty _displayGizmoProp;
       private SerializedProperty _displayNodesProp;
       private SerializedProperty _displayConnectionsProp;
       private SerializedProperty _allowLoggingProp;
       private SerializedProperty _nodeSphereSizeProp;

       private GUISkin _inspectorSkin;
       private GUISkin _sceneSkin;

       private PlatformPathfindingManager _platformPFManager;
       private bool _foldoutAllNodesList = false;
       
       private void RenameNodes()
       {
            int temp = 1;
            for(int i = 0; i < _platformPFManager.allNodes.Count; i++)
            {
                if(_platformPFManager.allNodes[i] == null)
                {
                    continue;
                }
                _platformPFManager.allNodes[i].transform.name = temp.ToString();
                temp++;
            }
       }

       private void DeleteAllNodes()
       {
            while(_platformPFManager.allNodes.Count > 0)
            {
                var node = _platformPFManager.allNodes[_platformPFManager.allNodes.Count - 1];
                _platformPFManager.allNodes.Remove(node);
                GameObject.DestroyImmediate(node.gameObject);
            }
       }
       private void OnEnable() 
       {
            
            _platformPFManager = target as PlatformPathfindingManager;
            _heuristicTypeProp = serializedObject.FindProperty("heuristicType");
            _calculatorPatienceProp = serializedObject.FindProperty("calculatorPatience");
            _allNodesProp = serializedObject.FindProperty("allNodes");
            _displayGizmoProp = serializedObject.FindProperty("displayGizmo");
            _displayNodesProp = serializedObject.FindProperty("displayNodes");
            _displayConnectionsProp = serializedObject.FindProperty("displayConnections");
            _allowLoggingProp = serializedObject.FindProperty("allowLogging");
            _nodeSphereSizeProp = serializedObject.FindProperty("nodeSphereSize");
            
            _inspectorSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector);
            _sceneSkin = EditorGUIUtility.GetBuiltinSkin(EditorSkin.Scene);

       }

        private void OnSceneGUI() 
        {
            if(!_platformPFManager.displayGizmo)
            {
                return;
            }

            //Debug.Log("Showing nodes");
            for(int i = 0; i < _platformPFManager.allNodes.Count; i++)
            {
                Node current = _platformPFManager.allNodes[i];
                if(current == null)
                {
                    continue;
                }
                if(_platformPFManager.displayNodes)
                {
                    Handles.color = Color.green;
                    Handles.DrawSolidDisc(current.transform.position, -Vector3.forward, _platformPFManager.nodeSphereSize);
                    Handles.Label(current.transform.position, current.name, _sceneSkin.textField);
                }
                if(_platformPFManager.displayConnections)
                {
                    Handles.color = Color.white;
                    foreach(Node neighbour in current.Neighbours)
                    {
                        Handles.DrawLine(current.transform.position, neighbour.transform.position);
                    }
                }
            }
        }


       public override void OnInspectorGUI()
       {
            var lastActiveSceneView = SceneView.lastActiveSceneView;
            serializedObject.Update();
            //Debug.Log("Showing pathfinding properties");

            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Heuristic Type: ",_inspectorSkin.label);
            EditorGUI.BeginChangeCheck();
            var heuristicType = (HeuristicType)EditorGUILayout.EnumPopup((HeuristicType)_heuristicTypeProp.enumValueFlag);
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target,"Heuristic Type");
                _platformPFManager.heuristicType = heuristicType;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Calculator Patience: ",_inspectorSkin.label);
            EditorGUI.BeginChangeCheck();
            int calculatorPatience = EditorGUILayout.IntField(_calculatorPatienceProp.intValue);
            if(EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Calculator Patience");
                _platformPFManager.calculatorPatience = calculatorPatience;
            }

            EditorGUILayout.EndHorizontal();

            _foldoutAllNodesList = EditorGUILayout.Foldout(_foldoutAllNodesList,"All Nodes: ");
            if(_foldoutAllNodesList)
            {       
                if(_platformPFManager.allNodes.Count == 0)
                {
                    if(GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.ExpandWidth(true)))
                    {
                        Vector3 newPosition = lastActiveSceneView.camera.transform.position;
                        GameObject newNodeGO = new GameObject("Node");
                        newNodeGO.transform.parent = _platformPFManager.transform;
                        Node newNodeComp = newNodeGO.AddComponent<Node>();
                        _platformPFManager.allNodes.Add(newNodeComp);
                        serializedObject.ApplyModifiedProperties();
                    }
                }
                for(int i = 0; i < _allNodesProp.arraySize; i++)
                {
                    SerializedProperty nodeProp = _allNodesProp.GetArrayElementAtIndex(i);
                    EditorGUILayout.BeginHorizontal();
                    EditorGUI.BeginChangeCheck();
                    Node value =  (Node)EditorGUILayout.ObjectField(nodeProp.objectReferenceValue,typeof(Node),true);
                    if(EditorGUI.EndChangeCheck())
                    {

                        _platformPFManager.allNodes[i] = value;
                        Undo.RecordObject(_platformPFManager, "Changed Node property");
                        SceneView.RepaintAll();
                        EditorUtility.SetDirty(_platformPFManager);
                    }

                    if(GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Plus"), GUILayout.ExpandWidth(false)))
                    {
                        if(value != null)
                        {
                            Debug.Log("Add new node above to previous one");
                            //_allNodesProp.InsertArrayElementAtIndex(i + 1);
                            Vector3 newPosition = value.transform.position + Vector3.up * 5f;
                            GameObject newNodeGO = new GameObject(value.transform.name + (i+1).ToString());
                            newNodeGO.transform.position = newPosition;
                            newNodeGO.transform.parent = _platformPFManager.transform;
                            newNodeGO.transform.SetSiblingIndex(value.transform.GetSiblingIndex() + 1);
                            Node newNodeComp = newNodeGO.AddComponent<Node>();
                            value.AddNeighbour(newNodeComp);
                            newNodeComp.AddNeighbour(value);
                            _platformPFManager.allNodes.Insert(i + 1, newNodeComp);
                            serializedObject.ApplyModifiedProperties();
                        }
                    }

                    if(GUILayout.Button(EditorGUIUtility.IconContent("d_Toolbar Minus"), GUILayout.ExpandWidth(false)))
                    {
                        if(value != null)
                        {
                            foreach(Node neighbour in value.Neighbours)
                            {
                                neighbour.RemoveNeighbour(value);
                            }
                        }
                        //_allNodesProp.DeleteArrayElementAtIndex(i);
                        
                        _platformPFManager.allNodes.Remove(value);
                        GameObject.DestroyImmediate(value.gameObject);
                        serializedObject.ApplyModifiedProperties();
                        i--;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            
            }
            
            EditorGUILayout.BeginHorizontal();
            
            if(GUILayout.Button("Clear All", GUILayout.ExpandWidth(true)))
            {
                EditorGUI.BeginChangeCheck();
                DeleteAllNodes();
                if(EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_platformPFManager,"Clear All Nodes");
                    
                    serializedObject.Update();
                }
                
            }


            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(_displayGizmoProp);
            EditorGUILayout.PropertyField(_displayNodesProp);
            EditorGUILayout.PropertyField(_displayConnectionsProp);
            EditorGUILayout.PropertyField(_allowLoggingProp);

            

            serializedObject.ApplyModifiedProperties();
            RenameNodes();
       }

       
    }
}
