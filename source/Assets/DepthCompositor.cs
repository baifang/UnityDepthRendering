/*
 * The MIT License (MIT)
 * Copyright (c) 2014 Baifang Lu (baifang@gmail.com)
 * 08/20/2014: Initial version.
 */

using UnityEngine;
using System.Collections;

public class DepthCompositor : MonoBehaviour {

    // Use this for initialization
    void Start () {
        _maskDepthRender = (DepthRender)GameObject.Find("MaskDepthRender").GetComponent("DepthRender");
        _sceneDepthRender = (DepthRender)GameObject.Find("SceneDepthRender").GetComponent("DepthRender");

        if (_depthCompositorMaterial == null)
        {
            _depthCompositorMaterial = new Material(Shader.Find("depth based compositor"));
            _depthCompositorMaterial.name = "$depthCompositorMaterial";
        }
    }

    // Update is called once per frame
    void Update () {

    }
    
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_maskDepthRender == null || _sceneDepthRender == null || _depthCompositorMaterial == null)
        {
            Graphics.Blit(src, dest);
        }
        else
        {
            _depthCompositorMaterial.SetTexture("maskDepthTexture", _maskDepthRender.depthTexture);
            _depthCompositorMaterial.SetTexture("sceneDepthTexture", _sceneDepthRender.depthTexture);
            Graphics.Blit(src, dest, _depthCompositorMaterial, 0);
        }
    }
    private DepthRender _maskDepthRender;
    private DepthRender _sceneDepthRender;
    private Material _depthCompositorMaterial;
}
