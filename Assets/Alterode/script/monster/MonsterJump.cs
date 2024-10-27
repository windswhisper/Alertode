using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TrueSync;

public class MonsterJump : MonsterWalk
{
    int jumpRange;
    FP jumpCd;

    FP jumpTime = 0;
    FP jumpSpeed = 5;
    bool isJumping = false;
    Coord jumpTarget;
    bool jumpAnimPlaying;

    public override void Init(MonsterData data){
        base.Init(data);

        jumpRange = data.paras[0];
        jumpCd = new FP(data.paras[1])/100;
    }

    public override void Enter(TSVector p,TeamType team){
        base.Enter(p,team);

        jumpTime = jumpCd;
    }


    public override void Move(FP dt){
        jumpTime-=dt;
        if(jumpTime<0){
            if(Jump()){
                jumpTime = jumpCd;
                isJumping = true;
                nextStep = null;
                coord = null;
                jumpSpeed = TSVector.Distance(new TSVector(jumpTarget.x,0,jumpTarget.y) , position)/7*10;
                fireCd = new FP(2)/3;
                jumpAnimPlaying = false;
                var dir = (new TSVector(jumpTarget.x,0,jumpTarget.y) - position).normalized;
                yRotation = Utils.VectorToAngle(dir);
                return;
            }
        }

        if(isJumping){
            if(!jumpAnimPlaying && view!=null)view.PlaySpellAnim();
            jumpAnimPlaying = true;

            var dir = (new TSVector(jumpTarget.x,0,jumpTarget.y) - position).normalized;
            var dis = TSVector.Distance(new TSVector(jumpTarget.x,0,jumpTarget.y) , position);
            yRotation = Utils.VectorToAngle(dir);
            if(dis < dt * jumpSpeed){
                isJumping = false;
                fireCd = new FP(1)/2;
            }
            else{
                position = position + dir * dt * jumpSpeed;
            }
        }
        else{
            Walk(dt);
        }
    }

    bool Jump(){
        var coord = new Coord(position.x,position.z);
        var map = BattleMap.ins;
        var pathCur = map.pathMapWalk[map.IndexByCoord(coord)];
        var list = new List<Coord>();
        var maxDis = 0;
        for(var y=0;y<map.mapData.height;y++){
            for(var x=0;x<map.mapData.width;x++){
                if(TSMath.Abs(x-coord.x)+TSMath.Abs(y-coord.y)>jumpRange)continue;
                if(map.pathMapWalk[map.IndexByCoord(x,y)]<0)continue;
                if(map.buildMap[map.IndexByCoord(x,y)]!=0)continue;

                var dis = pathCur - map.pathMapWalk[map.IndexByCoord(x,y)];
                if(dis > 0){
                    list.Add(new Coord(x,y));
                    if(dis > maxDis){
                        maxDis = dis;
                    }
                }
            }
        }

        if(list.Count == 0)return false;

        for(var i=list.Count-1;i>=0;i--){
            var dis = pathCur - map.pathMapWalk[map.IndexByCoord(list[i])];
            if(dis!=maxDis)list.Remove(list[i]);
        }

        jumpTarget = list[TSRandom.Range(0,list.Count)];

        return true;
    }
}
