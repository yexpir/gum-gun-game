using Cinemachine;
using SalvadorNovo.CharacterRelated;
using UnityEngine;

namespace SalvadorNovo.Assets.Scripts.CharacterRelated.Camera
{
    public class CinemaBehaviour : MonoBehaviour
    {
        private CinemachineFreeLook cinemachine;

        [SerializeField] private float[] jumping_TOP_Screen_Dead_Bias = new float[2];
        [SerializeField] private float[] jumping_MID_Screen_Dead_Bias = new float[2];
        [SerializeField] private float[] jumping_BOT_Screen_Dead_Bias = new float[2];
    
        [SerializeField] private float[] default_TOP_Screen_Dead_Bias = new float[2];
        [SerializeField] private float[] default_MID_Screen_Dead_Bias = new float[2];
        [SerializeField] private float[] default_BOT_Screen_Dead_Bias = new float[2];

        private bool _doOnce;
        private void Awake() => cinemachine = GetComponent<CinemachineFreeLook>();
        private void Update()
        {
            switch (PlayerMove.Player.IsGrounded)
            {
                case false when !_doOnce:
                {
                    _doOnce = true;
                    print("Camera Jump");
                    cinemachine.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = jumping_TOP_Screen_Dead_Bias[0];
                    cinemachine.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_DeadZoneHeight = jumping_TOP_Screen_Dead_Bias[1];
                
                    cinemachine.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = jumping_MID_Screen_Dead_Bias[0];
                    cinemachine.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_DeadZoneHeight = jumping_MID_Screen_Dead_Bias[1];
                
                    cinemachine.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = jumping_BOT_Screen_Dead_Bias[0];
                    cinemachine.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_DeadZoneHeight = jumping_BOT_Screen_Dead_Bias[1];
                    break;
                }
                case true when _doOnce:
                {
                    _doOnce = false;
                    print("Camera Default");
                    cinemachine.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = default_TOP_Screen_Dead_Bias[0];
                    cinemachine.GetRig(0).GetCinemachineComponent<CinemachineComposer>().m_DeadZoneHeight = default_TOP_Screen_Dead_Bias[1];
                
                    cinemachine.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = default_MID_Screen_Dead_Bias[0];
                    cinemachine.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_DeadZoneHeight = default_MID_Screen_Dead_Bias[1];
                
                    cinemachine.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_ScreenY = default_BOT_Screen_Dead_Bias[0];
                    cinemachine.GetRig(2).GetCinemachineComponent<CinemachineComposer>().m_DeadZoneHeight = default_BOT_Screen_Dead_Bias[1];
                    break;
                }
            }
        }
    }
}
