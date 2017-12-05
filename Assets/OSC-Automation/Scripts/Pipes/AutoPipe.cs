using UnityEngine;
using System.Collections.Generic;
using System;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Pipes
{
    public abstract class AutoPipe : Pipe
    {
        //[SerializeField]
        protected Type _type = Type.INOUT;
        //[SerializeField]
        public override Type type
        {
            get
            {
                return _type;
            }
        }

        public abstract string pipeName
        {
            get;
        }

        bool plugged = false;
        protected Connector parent = null;

        void AutoPlug()
        {
            if (plugged)
            {
                UnplugAll();
            }
            if (parent != null)
            {
                if (parent.type == Type.IN)
                {
                    _direction = Direction.UPWARDS;
                }
                else if (parent.type == Type.OUT)
                {
                    _direction = Direction.DOWNWARDS;
                }
            }/*
            else
            {
                Debug.LogWarning("Dad is nul" + gameObject.name);
            }*/
            List<Connector> childs = new List<Connector>();
            for (int c = 0; c < transform.childCount; ++c)
            {
                Connector con = transform.GetChild(c).gameObject.GetComponent<Connector>();
                if (con != null)
                {
                    childs.Add(con);
                }
                /* else
                 {
                     Debug.LogWarning("child is not connector" + gameObject.name);
                 }*/
            }

            if (type == Type.IN || type == Type.INOUT)
            {
                if (direction == Direction.UPWARDS)
                {
                    foreach (Connector con in childs)
                    {
                        input.Add(con.id, con);
                    }
                }
                else// if (direction == Direction.DOWNWARDS)
                {
                    Connector con = parent;
                    if (con != null)
                    {
                        input.Add(con.id, con);
                    }

                }
            }
            if (type == Type.OUT || type == Type.INOUT)
            {
                if (direction == Direction.DOWNWARDS)
                {
                    foreach (Connector con in childs)
                    {
                        output.Add(con.id, con);
                    }
                }
                else//if(direction == Direction.UPWARDS)
                {
                    Connector con = parent;
                    if (con != null)
                    {
                        output.Add(con.id, con);
                    }
                }
            }
            plugged = true;
        }

        protected abstract void InitializePipe();
        protected abstract void InitializeConnections();
        // Use this for initialization
        protected void Awake()
        {
            Initialize();
        }

        /*
        public List<GameObject> inputs = new List<GameObject>();
        public List<GameObject> outputs = new List<GameObject>();
        public GameObject parentGO;
        */
        protected void Start()
        {
            parent = transform.parent.gameObject.GetComponent<Connector>();
            InitializeConnections();
            AutoPlug();
            /*
            foreach(Connector c in input.Values)
            {
                inputs.Add(((Pipe)c).gameObject);
            }
            foreach (Connector c in output.Values)
            {
                outputs.Add(((Pipe)c).gameObject);
            }
            if (parent != null)
            {
                parentGO = ((Pipe)parent).gameObject;
            }*/

            UpdateName();
        }
        /*
        string s = "■□▢▣▤▥▦▧▨▩▪▫▬▭▮▯▰▱▲△▴▵▶▷▸▹►▻▼▽▾▿◀◁◂◃◄◅◆◇◈◉◊○◌◍◎●◐◑◒◓◔◕◖◗◘◙◚◛◜◝◞◟◠◡◢◣◤◥◦◧◨◩◪◫◬◭◮◯◰◱◲◳◴◵◶◷◸◹◻◼◽◾◿";
        string s2 = "← → ↑ ↓ ↔ ↕ ↖ ↗ ↘ ↙ ↚ ↛ ↮ ⟵ ⟶ ⟷ 
        ⇐ ⇒ ⇑ ⇓ ⇔ ⇕ ⇖ ⇗ ⇘ ⇙ ⇍ ⇏ ⇎ ⟸ ⟹ ⟺
        ⇦ ⇨ ⇧ ⇩ ⬄ ⇳ ⬀ ⬁ ⬂ ⬃ ⬅ ( ⮕ ➡ ) ⬆ ⬇ ⬈ ⬉ ⬊ ⬋ ⬌ ⬍ 
        ⇆ ⇄ ⇅ ⇵ ⇈ ⇊ ⇇ ⇉  ⬱ ⇶  ⇠ ⇢ ⇡ ⇣ ⇚ ⇛ ⤊ ⤋ ⭅ ⭆ ⟰ ⟱ 
        ↢ ↣  ↼ ⇀ ↽ ⇁ ↿ ↾ ⇃ ⇂ ⇋ ⇌  ⟻ ⟼  ⇽ ⇾ ⇿  ⇜ ⇝  ⬳ ⟿ ⥊ ⥋ ⥌ ⥍ ⥎ ⥏ ⥐ ⥑  ⥒ ⥓ ⥔ ⥕ ⥖ ⥗ ⥘ ⥙ ⥚ ⥛ ⥜ ⥝ ⥞ ⥟ ⥠ ⥡  
        ⥢ ⥤ ⥣ ⥥ ⥦ ⥨ ⥧ ⥩ ⥮ ⥯ ⥪ ⥬ ⥫ ⥭  ↤ ↦ ↥ ↧  ⇤ ⇥ ⤒ ⤓ ↨  ↞ ↠ ↟ ↡ ⇷ ⇸ ⤉ ⤈ ⇹  ⇺ ⇻ ⇞ ⇟ ⇼  ⬴ ⤀ ⬵ ⤁  ⬹ ⤔ ⬺ ⤕  ⤂ ⤃ ⤄  ⬶ ⤅  ⬻ ⤖  ⬷ ⤐ ⬼ ⤗ ⬽ ⤘  ⤆ ⤇  ⤌ ⤍ ⤎ ⤏  ⬸ ⤑ ⤝ ⤞ ⤟ ⤠  ⤙ ⤚ ⤛ ⤜ ⤡ ⤢ ⤣ ⤤ ⤥ ⤦ ⤪ ⤨ ⤧ ⤩ ⤭ ⤮ ⤯ ⤰ ⤱ ⤲ ⤫ ⤬ ↰ ↱ ↲ ↳ ⬐ ⬎ ⬑ ⬏ ↴ ↵  ⤶ ⤷ ⤴ ⤵  ↩ ↪ ↫ ↬ ⥼ ⥽ ⥾ ⥿ ⥂ ⥃ ⥄ ⭀ ⥱ ⥶ ⥸ ⭂ ⭈ ⭊ ⥵ ⭁ ⭇ ⭉ ⥲ ⭋ ⭌ ⥳ ⥴ ⥆ ⥅ ⥹ ⥻  ⬰ ⇴ ⥈ ⬾ ⥇ ⬲ ⟴  ⥷ ⭃ ⥺ ⭄  ⇱ ⇲ ↸ ↹ ↯ ↭ ⥉ ⥰  ⬿ ⤳  ↜ ↝  ⤼ ⤽ ↶ ↷ ⤾ ⤿ ⤸ ⤹ ⤺ ⤻  ↺ ↻ ⥀ ⥁ ⟲ ⟳"
            string s3 = "➩ ➪ ➫ ➬ ➭ ➮ ➯ ➱";
        string s4 = "⇪ ⇫ ⇬ ⇭ ⇮ ⇯  ➳ ➵ ➴ ➶ ➸ ➷ ➹  ➙ ➘ ➚ ➾ ⇰  ➛ ➜ ➔ ➝ ➞ ➟ ➠ ➥ ➦ ➧ ➨ ➲ ➢ ➣ ➤  ➺ ➻ ➼ ➽";*/

        protected void UpdateName()
        {
            string tag = "";
            string postTag = "";
            if (type == Type.IN)
            {
                if (direction == Direction.UPWARDS)
                {
                    tag += "⇦";
                }
                else
                {
                    tag += "◇";
                    //       postTag += "⇨";
                }
            }
            else if (type == Type.INOUT)
            {
                if (direction == Direction.UPWARDS)
                {
                    if (parent != null)
                    {
                        tag += "⇖";
                    }
                    else
                    {
                        tag += "⇦";
                    }
                }
                else
                {
                    if (parent != null)
                    {
                        tag += "⇘";
                    }
                    else
                    {
                        tag += "➩";
                    }
                    
                }
            }
            else if (type == Type.OUT)
            {
                if (direction == Direction.DOWNWARDS)
                {
                    tag += "◇";
                    //     postTag += "⇘";
                }
                else
                {
                    tag += "⇧";
                    //   postTag += "⇦";
                }

            }
            gameObject.name = tag + " " + pipeName + " " + postTag;

        }

        protected void OnValidate()
        {
            if (transform != null)
            {
                parent = transform.parent.gameObject.GetComponent<Connector>();
            }
            Initialize();
        }

        void Initialize()
        {
            InitializePipe();
            UpdateName();
        }
#if UNITY_EDITOR
        public static T CreateGameObjectWithComponent<T>(MenuCommand menuCommand) where T : AutoPipe
        {

            // Create a custom game object
            GameObject go = new GameObject();
            T component = go.AddComponent<T>();
            component.parent = (menuCommand.context as GameObject).GetComponent<Connector>();
            // Ensure it gets reparented if this was a context click (otherwise does nothing)
            GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
            // Register the creation in the undo system
            Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
            Selection.activeObject = go;
            component.Initialize();
            return component;
        }
#endif
    }

}