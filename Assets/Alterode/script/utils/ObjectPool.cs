using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
	public static ObjectPool ins;

	public Dictionary<string,Queue<GameObject>> bulletsPool;
	public Dictionary<string,Queue<GameObject>> effectsPool;
	public Dictionary<string,Queue<GameObject>> buffsPool;

	public static void Init(){
		ins = new ObjectPool();

		ins.bulletsPool = new Dictionary<string,Queue<GameObject>>();
		ins.effectsPool = new Dictionary<string,Queue<GameObject>>();
		ins.buffsPool = new Dictionary<string,Queue<GameObject>>();
	}

	public void PutBullet(BulletView bulletView){
		bulletView.gameObject.SetActive(false);

		if(!bulletsPool.ContainsKey(bulletView.bullet.name)){
			bulletsPool.Add(bulletView.bullet.name,new Queue<GameObject>());
		}
		bulletsPool[bulletView.bullet.name].Enqueue(bulletView.gameObject);
	}

	public GameObject GetBullet(string name){
		if(!bulletsPool.ContainsKey(name))return null;
		if(bulletsPool[name].Count == 0)return null;

		var bullet = bulletsPool[name].Dequeue();
		bullet.gameObject.SetActive(true);
		return bullet;
	}


	public void PutEffect(EffectView effectView){
		effectView.gameObject.SetActive(false);

		if(!effectsPool.ContainsKey(effectView.effectName)){
			effectsPool.Add(effectView.effectName,new Queue<GameObject>());
		}
		effectsPool[effectView.effectName].Enqueue(effectView.gameObject);
	}

	public GameObject GetEffect(string name){
		if(!effectsPool.ContainsKey(name))return null;
		if(effectsPool[name].Count == 0)return null;

		var effect = effectsPool[name].Dequeue();
		if(effect==null)return null;
		effect.SetActive(true);
		return effect;
	}

	public void PutBuff(BuffView buffView){
		buffView.gameObject.SetActive(false);

		if(!buffsPool.ContainsKey(buffView.buffName)){
			buffsPool.Add(buffView.buffName,new Queue<GameObject>());
		}
		buffsPool[buffView.buffName].Enqueue(buffView.gameObject);
	}

	public GameObject GetBuff(string name){
		if(!buffsPool.ContainsKey(name))return null;
		if(buffsPool[name].Count == 0)return null;

		var buff = buffsPool[name].Dequeue();
		if(buff==null)return null;
		buff.SetActive(true);
		return buff;
	}

}
