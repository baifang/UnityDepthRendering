/*
 * The MIT License (MIT)
 * Copyright (c) 2014 Baifang Lu (baifang@gmail.com)
 * 08/20/2014: Initial version.
 */

using UnityEngine;
using System.Collections;

public class DepthRender : MonoBehaviour {

    public enum DepthObjects
    {
        Mask, 
        Scene
    }
    public DepthObjects depthMode;

    [Range(8, 31)]
    public int MaskLayer = 31;
    public RenderTexture depthTexture
    {
        //set { _sceneDepthTexture = sceneDepthTexture; }
        get { return _depthTexture; }
    }

    // Use this for initialization
    void Start () {
        _camera = (Camera)gameObject.GetComponent("Camera");
        if (_camera == null)
        {
            _camera = (Camera)gameObject.AddComponent("Camera");
        }
    
        _camera.enabled = false;
        _camera.clearFlags = CameraClearFlags.Color;
        _camera.backgroundColor = Color.white;

        if (depthMode == DepthObjects.Mask)
            _camera.cullingMask = (1 << MaskLayer);
        else
            _camera.cullingMask = ~(1 << MaskLayer) & _camera.cullingMask;

        _depthTexture = CreateScreenRenderTexture();
        _camera.targetTexture = _depthTexture;

        if (_depthRenderShader == null)
        {
            _depthRenderShader = Shader.Find("Custom/buildin depth shader");
        }
    }

    // Update is called once per frame
    void Update () {
        if (_screenWidth != Screen.width || _screenHeight != Screen.height)
        {
            RenderTexture rt = _camera.targetTexture;
            _depthTexture = CreateScreenRenderTexture();
            _camera.targetTexture = _depthTexture;
            Object.Destroy(rt);
        }

        if (_depthRenderShader != null)
        {
            _camera.RenderWithShader(_depthRenderShader, "");
        }
        else
        {
            _camera.Render();
        } 
    }

    RenderTexture CreateScreenRenderTexture()
    {
        _screenWidth = Screen.width;
        _screenHeight = Screen.height;
        RenderTexture rt;
        rt = new RenderTexture(Screen.width, Screen.height, 24, RenderTextureFormat.ARGB32);
        rt.name = "$" + gameObject.name +"@" + Screen.width + "x" + Screen.height;
        return rt;
    }
    private Camera _camera = null;
    private RenderTexture _depthTexture = null;
    private int _screenWidth = 1;
    private int _screenHeight = 1;
    private Shader _depthRenderShader;
}
