using System.Linq;
using UnityEditor.Animations;
using UnityEngine;

namespace Util
{
    /// <summary>
    /// ��animator���ж��η�װ����չ����
    /// </summary>
    public static class AnimatorUtil     
    {
        /// <summary>
        /// ��ȡ��ǰ�㼶����״̬
        /// </summary>
        /// <param name="ani"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static AnimatorState[] GetAnimatorState(this Animator ani, int layer)
        {
            var machine = ani.GetAnimatorStateMachine(layer);
            return machine.GetAnimatorState();
        }

        /// <summary>
        /// ��ȡ��ǰanimator��Ӧ�㼶��״̬��
        /// </summary>
        /// <param name="ani"></param>
        /// <param name="layer"></param>
        /// <returns></returns>
        public static AnimatorStateMachine GetAnimatorStateMachine(this Animator ani,int layer)
        {
            AnimatorController aniController = ani.runtimeAnimatorController as AnimatorController;
            return aniController.layers[layer].stateMachine;
        }

        /// <summary>
        /// ��ȡ��״̬��ʼ����
        /// </summary>
        /// <param name="ani"></param>
        /// <param name="baseLayer"></param>
        /// <returns></returns>
        public static AnimatorStateMachine[] GetSubStateMachines(this Animator ani, int baseLayer)
        {
            var baseMachine = ani.GetAnimatorStateMachine(baseLayer);
            return baseMachine.stateMachines.Select(u=>u.stateMachine).ToArray();
        }

        /// <summary>
        /// ��ȡ��ǰ״̬������״̬
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public static AnimatorState[] GetAnimatorState(this AnimatorStateMachine machine)
        {
            return machine.states.Select(u => u.state).ToArray();
        }

        /// <summary>
        /// ͨ����״̬�����ƻ�ȡ��״̬��
        /// </summary>
        /// <param name="ani"></param>
        /// <param name="baseLayer"></param>
        /// <param name="stateMachineName"></param>
        /// <returns></returns>
        public static AnimatorStateMachine GetSubStateMachine(this Animator ani, int baseLayer, string stateMachineName)
        {
            var machines = ani.GetSubStateMachines(baseLayer);
            AnimatorStateMachine machine = machines.FirstOrDefault(u => u.name == stateMachineName);
            if (machine == null)
            {
                Debug.LogError("δ�ҵ�����Ϊ"+ stateMachineName+"����״̬��");
            }
            return machine;
        }

        /// <summary>
        /// �Ƴ���ǰ�����еĹ���״̬
        /// </summary>
        /// <param name="ani"></param>
        /// <param name="layer"></param>
        public static void RemoveAllTrasition(this Animator ani, int layer)
        {
            var states = ani.GetAnimatorState(layer);
            foreach (AnimatorState state in states)
            {
                state.RemoveStateAllTrasition();
            }
        }

        /// <summary>
        /// �Ƴ�ָ��״̬�����й���״̬
        /// </summary>
        /// <param name="ani"></param>
        /// <param name="state"></param>
        public static void RemoveStateAllTrasition(this AnimatorState state)
        {
            foreach (AnimatorStateTransition transition in state.transitions)
            {
                state.RemoveTransition(transition);
            }
        }

        /// <summary>
        /// ͨ��Ŀ��״̬�����ƹ�ϣֵ��ȡ����״̬
        /// </summary>
        /// <param name="state"></param>
        /// <param name="targetNameHash"></param>
        /// <returns></returns>
        public static AnimatorStateTransition GetTransition(this AnimatorState state,int targetNameHash)
        {
            var transition = state.transitions.FirstOrDefault(u => u.destinationState.nameHash == targetNameHash);
            if (transition == null)
            {
                Debug.LogError("�޷��ҵ�NameHashΪ" + targetNameHash + "��״̬");
            }
            return transition;
        }
    }
}