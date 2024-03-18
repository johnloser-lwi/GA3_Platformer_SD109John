using System;
using UnityEngine;

namespace Camera
{
    [RequireComponent(typeof(Cinemachine.CinemachineVirtualCamera))]
    public class VirtualCameraController : MonoBehaviour
    {
        private void Start()
        {
            // Just in case the virtual camera is not following anything, try to find the player and set it as the target
            var cinemachineVirtualCamera = GetComponent<Cinemachine.CinemachineVirtualCamera>();
            if (cinemachineVirtualCamera.Follow != null && cinemachineVirtualCamera.LookAt != null) return;
            
            var player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                cinemachineVirtualCamera.Follow = player.transform;
                cinemachineVirtualCamera.LookAt = player.transform;
            }
        }
    }
}