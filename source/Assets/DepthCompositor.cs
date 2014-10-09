/*
 * The MIT License (MIT)
 * Copyright (c) 2014 Baifang Lu (baifang@gmail.com)
 * 08/20/2014: Initial version.
 */

using UnityEngine;
using System.Collections;

public class DepthCompositor : MonoBehaviour {

    [Range(8, 31)]
    public int MaskLayer = 31;

    // Use this for initialization
    void Start () {
        _refCamera = (Camera)GetComponent("Camera");

        _maskDepthRenderGameObject = new GameObject("$MaskDepthRender");
        _maskDepthRenderCamera = (Camera)_maskDepthRenderGameObject.AddComponent("Camera");
        _maskDepthRenderCamera.enabled = false;

        _sceneDepthRenderGameObject = new GameObject("$SceneDepthRender");
        _sceneDepthRenderCamera = (Camera)_sceneDepthRenderGameObject.AddComponent("Camera");
        _sceneDepthRenderCamera.enabled = false;

        if (_depthCompositorMaterial == null)
        {
            _depthCompositorMaterial = new Material(Shader.Find("depth based compositor"));
            _depthCompositorMaterial.name = "$depthCompositorMaterial";
        }

        if (_depthRenderShader == null)
        {
            _depthRenderShader = Shader.Find("Custom/buildin depth shader");
        }
    }

    // Update is called once per frame
    void Update () {

    }

    void OnPreCull()
    {
        if (_depthRenderShader == null)
            return;

        if (_screenWidth != Screen.width || _screenHeight != Screen.height)
        {
            //RenderTexture rt = _maskDepthRenderCamera.targetTexture;
            _maskDepthRenderTexture = CreateScreenRenderTexture("mask");
            //Object.Destroy(rt);

            //RenderTexture rt = _maskDepthRenderCamera.targetTexture;
            _sceneDepthRenderTexture = CreateScreenRenderTexture("scene");
            //Object.Destroy(rt);

            _finalRenderTexture = CreateScreenRenderTexture("composited");
        }

        if (_refCamera != null && _finalRenderTexture != null)
        {
            _refCamera.targetTexture = _finalRenderTexture;
        }

        if (_maskDepthRenderCamera != null && _maskDepthRenderTexture !=null)
        {
            _maskDepthRenderCamera.CopyFrom(_refCamera);
            _maskDepthRenderCamera.cullingMask = 1 << MaskLayer;
            _maskDepthRenderCamera.targetTexture = _maskDepthRenderTexture;
            _maskDepthRenderCamera.clearFlags = CameraClearFlags.Color;
            _maskDepthRenderCamera.backgroundColor = Color.white;
            _maskDepthRenderCamera.RenderWithShader(_depthRenderShader, "");
        }

        if (_sceneDepthRenderCamera != null && _sceneDepthRenderTexture != null)
        {
            _sceneDepthRenderCamera.CopyFrom(_refCamera);
            _sceneDepthRenderCamera.cullingMask = _sceneDepthRenderCamera.cullingMask & ~(1 << MaskLayer);
            _sceneDepthRenderCamera.targetTexture = _sceneDepthRenderTexture;
            _sceneDepthRenderCamera.clearFlags = CameraClearFlags.Color;
            _sceneDepthRenderCamera.backgroundColor = Color.white;
            _sceneDepthRenderCamera.RenderWithShader(_depthRenderShader, "");
        }
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_sceneDepthRenderTexture == null || _maskDepthRenderTexture == null || _depthCompositorMaterial == null)
        {
            Graphics.Blit(src, dest);
        }
        else
        {
            _depthCompositorMaterial.SetTexture("maskDepthTexture", _maskDepthRenderTexture);
            _depthCompositorMaterial.SetTexture("sceneDepthTexture", _sceneDepthRenderTexture);
            Graphics.Blit(src, dest, _depthCompositorMaterial, 0);
        }
    }
    
    RenderTexture CreateScreenRenderTexture(string name)
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        RenderTexture rt;
        rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        rt.name = "$" + name + "@" + Screen.width + "x" + Screen.height;
        return rt;
    }

    private Camera _refCamera;

    private Camera _maskDepthRenderCamera;
    private Camera _sceneDepthRenderCamera;
    private GameObject _maskDepthRenderGameObject;
    private GameObject _sceneDepthRenderGameObject;
    private RenderTexture _maskDepthRenderTexture;
    private RenderTexture _sceneDepthRenderTexture;

    private RenderTexture _finalRenderTexture;

    private Material _depthCompositorMaterial;

    private Shader _depthRenderShader;
    private int _screenWidth = 1;
    private int _screenHeight = 1;
}
