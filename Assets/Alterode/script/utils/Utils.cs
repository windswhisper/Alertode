using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TrueSync;

//网格坐标类
public class Coord{
    public int x = 0;
    public int y = 0;

    public Coord(int x,int y){
        this.x = x;
        this.y = y;
    }
    public Coord(float x,float y){
        this.x = (int)Mathf.Round(x);
        this.y = (int)Mathf.Round(y);
    }
    public Coord(FP x,FP y){
        this.x = (int)TSMath.Round(x);
        this.y = (int)TSMath.Round(y);
    }

    public Coord(TSVector v){
        this.x = (int)TSMath.Round(v.x);
        this.y = (int)TSMath.Round(v.z);
    }


    public bool EqualTo(Coord c){
        return c.x == x && c.y == y;
    }
}


public class Utils 
{
    public static float _LerpRate = 30.0f;
    public static string charaTable = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

    public static string CreateUserId(){
        if(GlobalData.tapUserProfile!=null)
            return GlobalData.tapUserProfile.openid;
        return "a"+UserData.data.joinTime+CreateRandomStr(4);
    }

    public static string CreateRandomStr(int len){
        var sb = new StringBuilder(len);
        var rand = new System.Random();
        for(var i=0;i<len;i++){
            var p = rand.Next()%charaTable.Length;
            sb.Append(charaTable[p]);
        }
        return sb.ToString();
    }

    /*平滑旋转*/
    public static void RotateSmooth(Transform transform,Vector3 rotation,float rate){
	    var rawRotation = transform.rotation;
        var aimRotation = Quaternion.Euler(rotation);

        float rotateAngle = Quaternion.Angle(rawRotation, aimRotation);
        if(rotateAngle<0.1f){
            transform.rotation = aimRotation;
        }
        else{
            transform.rotation = Quaternion.Lerp(rawRotation, aimRotation, rate);
        }
    }
    
    /*平滑旋转2*/
    public static void LookAtSmooth(Transform transform,Vector3 aimPos,float rate){
        var rawRotation = transform.rotation;
        transform.LookAt(aimPos);
        var lookatRotation = transform.rotation;
        transform.rotation = rawRotation; 

        float rotateAngle = Quaternion.Angle(rawRotation, lookatRotation);
        if(rotateAngle<0.1f){
            transform.rotation = lookatRotation;
        }
        else{
            transform.rotation = Quaternion.Lerp(rawRotation, lookatRotation, rate);
        }
    }

    /*计算向量水平方向的旋转角度，z轴为起始方向*/
    public static FP VectorToAngle(TSVector v){
        if(v.z>0){
            return TSMath.Atan(v.x/v.z);
        }
        else if(v.z==0){
            if(v.x>0)
                return TSMath.Pi/2;
            else
                return -TSMath.Pi/2;
        }
        else{
            if(v.x>0)
                return TSMath.Atan(v.x/v.z) + TSMath.Pi;
            else
                return TSMath.Atan(v.x/v.z) - TSMath.Pi;
        }
    }

    /*将水平的旋转角度转为单位向量，z轴为起始方向*/
    public static TSVector AngleToVector(FP a)
    {
        return new TSVector(TSMath.Sin(a), 0, TSMath.Cos(a));
    }

    /*在水平面上旋转向量*/
    public static TSVector VectorRotate(TSVector v,FP w){
        return new TSVector(v.x*TSMath.Cos(w)-v.z*TSMath.Sin(w),v.y,v.x*TSMath.Sin(w)+v.z*TSMath.Cos(w));
    } 

    /*在垂直方向上旋转向量*/
    public static TSVector VectorRaise(TSVector v,FP w){
        var d = TSMath.Sqrt(v.x*v.x+v.z*v.z);
        return new TSVector(v.x*TSMath.Cos(w),d*TSMath.Sin(w),v.z*TSMath.Cos(w));
    } 

    /*计算两个向量的叉积*/
    public static FP VectorCross(TSVector2 v1,TSVector2 v2){
        return v1.x*v2.y - v2.x*v1.y;
    } 

    /*判断点是否在4边形内*/
    public static bool IsPointInRect(TSVector2 p,TSVector2 a,TSVector2 b,TSVector2 c,TSVector2 d){
        var ab = a-b;
        var ap = a-p;
        var cd = c-d;
        var cp = c-p;

        var da = d-a;
        var dp = d-p;
        var bc = b-c;
        var bp = b-p;

        bool isBetweenAB_CD = VectorCross(ab,ap)*VectorCross(cd,cp)>0;
        bool isBetweenDA_BC = VectorCross(da,dp)*VectorCross(bc,bp)>0;
        return isBetweenAB_CD && isBetweenDA_BC;

    }

    /*定点数向量转普通向量*/
    public static Vector3 TSVecToVec3(TSVector v){
        return new Vector3((float)v.x,(float)v.y,(float)v.z);
    }

    /*按时间间隔计算平滑率*/
    public static float LerpByTime(float dt){
        var rate = dt*_LerpRate;
        return rate>1?1:rate;
    }

    /*判断网格点是否在地图内部*/
    public static bool IsCoordInMap(Coord c){
        return IsCoordInMap(c.x,c.y);
    }
    public static bool IsCoordInMap(int x,int y){
        return x>=0 && y>=0 && x<BattleMap.ins.mapData.width && y<BattleMap.ins.mapData.height;
    }
    public static bool IsCoordInMap(Coord c,int w,int h){
        return IsCoordInMap(c.x,c.y,w,h);
    }
    public static bool IsCoordInMap(int x,int y,int w,int h){
        return x>=0 && y>=0 && x<w && y<h;
    }

    /*定点向量旋转*/
    public static TSVector TSRotate(TSVector from,TSVector to,FP angle){
        var q1 = TSQuaternion.Euler(from);
        var q2 = TSQuaternion.Euler(to);
        var q = TSQuaternion.RotateTowards(q1,q2,angle);
        return q.eulerAngles;
    }

    /*随机抽取一个芯片*/
    public static string RandomChip(){
        var poolNormal = new List<string>();
        var poolEpic = new List<string>();
        var poolLegend = new List<string>();

        foreach(var data in Configs.chipConfig.datas){
            if(data.rare == 0){
                poolNormal.Add(data.name);
            }
            else if(data.rare == 1){
                poolEpic.Add(data.name);
            }
            else if(data.rare == 2){
                poolLegend.Add(data.name);
            }
        }

        var weight = Configs.commonConfig.chipDropWeight;
        var totalWeight = weight[0]+weight[1]+weight[2];

        var r = TSRandom.instance.Next()%totalWeight;
        if(r < weight[0]){
            return poolNormal[TSRandom.Range(0,poolNormal.Count)];
        }
        else if(r < weight[0]+weight[1]){
            return poolEpic[TSRandom.Range(0,poolEpic.Count)];
        }
        else{
            return poolLegend[TSRandom.Range(0,poolLegend.Count)];
        }
    }

    /*随机抽取一个指定稀有度的芯片*/
    public static string RandomChip(int rare){
        var pool = new List<string>();

        foreach(var data in Configs.chipConfig.datas){
            if(!data.hero && data.rare == rare){
                pool.Add(data.name);
            }
        }

        return pool[TSRandom.Range(0,pool.Count)];
    }

    /*随机抽取一个适配建筑的芯片*/
    public static string RandomChip(string buildID,List<TechChipData> ownList){
        var banList = new List<TechChipData>();

        return RandomChip(buildID,banList,ownList);
    }

    /*随机抽取一个适配建筑的芯片，会屏蔽列表中已出现的*/
    public static string RandomChip(string buildID,List<TechChipData> banList,List<TechChipData> ownList){
        var pool = new List<string>();
        var buildData = Configs.GetBuild(buildID);

        bool onlyUpgrade = false;
        int count = 0;
        foreach(var tech in ownList){
            if(buildID == tech.buildID){
                count++;
            }
        }
        if(count>=3){
           onlyUpgrade=true;
        }

        foreach(var data in Configs.chipConfig.datas){
            if(!data.hero){
                bool isBan = false;
                bool isUpgrade = false;
                if(data.prerequisite!=""){
                    isBan = true;
                    foreach(var ownChip in ownList){
                        if(buildID == ownChip.buildID && data.prerequisite == ownChip.chipID){
                            isBan = false;
                            isUpgrade = true;
                        }
                    }
                }
                else if(onlyUpgrade){
                    isBan = true;
                }
                else{
                    foreach(var ownChip in ownList){
                        if(buildID == ownChip.buildID){
                            var ownChipData = Configs.GetChip(ownChip.chipID);
                            if(ownChipData.className == data.className){
                                isBan = true;
                            }
                        }
                    }
                }
                if(!isUpgrade && !IsChipMatchBuild(buildData,data))continue;
                foreach(var banChip in banList){
                    if(buildID == banChip.buildID && data.name == banChip.chipID){
                        isBan = true;
                        break;
                    }
                }
                if(isBan)continue;
                
                pool.Add(data.name);
            }
        }

        if(pool.Count==0)return "";


        return pool[TSRandom.Range(0,pool.Count)];
    }


    public static bool IsChipMatchBuild(BuildData buildData,ChipData chipData){
        bool isMatch = true;
        if(chipData.ownerBuilds.Count!=0){
            foreach(var build in chipData.ownerBuilds){
                if(build == buildData.name){
                    return true;
                }
            }
            return false;
        }
        else{
            foreach(var tag in chipData.requireTags){
                if(tag == "OnlyCustom"){
                    return false;
                }
            }

            foreach(var chip in buildData.defaultChips){
                if(chip == chipData.name){
                    return true;
                }
            }

            foreach(var tag in chipData.requireTags){
                if(!buildData.tags.Contains(tag)){
                    isMatch = false;
                     break;
                 }
            }
            foreach(var tag in chipData.negativeTags){
                if(buildData.tags.Contains(tag)){
                    isMatch = false;
                    break;
                }
            }
            foreach(var tag in chipData.negativeTags){
                if(BattleManager.ins.battleModifier.nagetiveBuildTag.Contains(tag)){
                    isMatch = false;
                    break;
                }
            }
        }
        return isMatch;
    } 

    public static List<T> RandomList<T>(List<T> list){ 
        var newList = new List<T>();
        foreach (var item in list) {
            newList.Insert(TSRandom.Range(0,newList.Count+1), item);
        }
        return newList;
    }
}
