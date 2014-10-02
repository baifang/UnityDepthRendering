UnityDepthRendering
===================
Render accurate depth information in Unity 3D

Depth information may play an important role on post rendering image effects. Most of graphics engine does not have interface to read depth data from graphics. Furthermore, even the hardware depth information is available, the depth is in non-linear window coordinate due to projection process. In these cases, rendering depth information into textures probably is the only solution. 

This project is to demonstrate the techniques on rendering linear depth information with Camera.RenderWithShader. For testing purpose, a simple project with depth based post rendering image effect is created. 


