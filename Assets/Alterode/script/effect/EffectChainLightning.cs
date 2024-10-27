using System.Collections.Generic;

using UnityEngine;

public class EffectChainLightning : MonoBehaviour {

    public float detail = 0.25f;//增加后，线条数量会减少，每个线条会更长。  

    public float displacement = 1;//位移量，也就是线条数值方向偏移的最大值  

    Vector3 startPos;
    Vector3 endPos;

    private LineRenderer _lineRender;  

    private List<Vector3> _linePosList; 

    float heightDelta = 0.2f; 


    private void Awake()  
    {          
    	_lineRender =GetComponent<LineRenderer>();  

        _linePosList = new List<Vector3>();  

        InvokeRepeating("UpdateSlow",0,0.03f);
    }  

    public void Init(Vector3 startPos,Vector3 endPos){
    	this.startPos = startPos;
    	this.endPos = endPos;
        heightDelta = Vector3.Distance(startPos,endPos)/5+0.2f;
    }

    void UpdateSlow()  
    {  

        if(Time.timeScale != 0)  
        {  
            _linePosList.Clear();  

            //获得开始点与结束点之间的随机生成点

            CollectLinPos(startPos,endPos, displacement);  

            _linePosList.Add(endPos);  

            //把点集合赋给LineRenderer

            _lineRender.SetVertexCount(_linePosList.Count);  

            for (int i = 0, n = _linePosList.Count; i< n; i++)  

            {  

                _lineRender.SetPosition(i, _linePosList[i]);  

            }  

        }  

    }  

    //收集顶点，中点分形法插值抖动  

    private void CollectLinPos(Vector3 startPos,Vector3 destPos,float displace)  

    {  

        //递归结束的条件

        if (displace < detail)  

        {  

            _linePosList.Add(startPos);  

        }  

        else  

        {  

            float midX = (startPos.x + destPos.x) / 2;  

            float midY = (startPos.y + destPos.y) / 2;  

            float midZ = (startPos.z + destPos.z) / 2;  

            midX += (float)(UnityEngine.Random.value - 0.5) * displace;  

            midY += (float)(UnityEngine.Random.value - 0.5 + heightDelta) * displace;  

            midZ += (float)(UnityEngine.Random.value - 0.5) * displace;  

            Vector3 midPos =new Vector3(midX,midY,midZ);  

            //递归获得点

            CollectLinPos(startPos,midPos, displace / 2);  

            CollectLinPos(midPos,destPos, displace / 2);  

        }  

    } 

}