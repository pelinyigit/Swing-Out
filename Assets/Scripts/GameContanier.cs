using System.Collections.Generic;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using System;

public class GameContanier : SingletonMonoBehaviour<GameContanier>
{
    public Action OnGameOver;
    public PipeEndController pipeEnd;
    public LightBeamController lightBeam;
    public List<RingController> rings;
    public GameObject finishParticle;
    public bool isParticleVFXAlreadyStarted;
    public bool isGameOver;
    [Range(1, 30)] public int maxPipeCount;

    [Header("Finish Particle Params"), Tooltip("Seconds in Virtual Time")]
    public float finishParticleDelayTime;
    public float finishParticleBetweenEach;

    private int activeRingIndex = 0;
    private List<GameObject> confettiParticles;

    protected override void Awake()
    {
        //Application.targetFrameRate = 60;
        base.Awake();
        DOTween.Init(true, true, LogBehaviour.ErrorsOnly);
    }

    private void Start()
    {
        OnGameOver += GameOver;
    }

    public void CheckGameStatus()
    {
        Debug.Log("Current: " + pipeEnd.currentPipeCount + " Max: " + maxPipeCount);
        if (pipeEnd.currentPipeCount >= maxPipeCount)
        {
            OnGameOver?.Invoke();
        }
    }

    private void GameOver()
    {
        isGameOver = true;
        GameManager.instance.LevelComplete();
        if (!isParticleVFXAlreadyStarted)
        {
            StartCoroutine(DoConfettiParticleVFX());
        }
    }

    public IEnumerator DoConfettiParticleVFX()
    {
        yield return new WaitForSeconds(finishParticleDelayTime);
        foreach (Transform currentConfetti in finishParticle.GetComponentInChildren<Transform>())
        {
            currentConfetti.gameObject.SetActive(true);
            yield return new WaitForSeconds(finishParticleBetweenEach);
        }
        yield return null;
    }

    public void TarnishCurrentRingColor()
    {
        activeRingIndex--;
        rings[activeRingIndex].TarnishColor();
    }

    public void ReverseCurrentRingColor()
    {
        rings[activeRingIndex].ReverseColor();
        activeRingIndex++;
    }
}