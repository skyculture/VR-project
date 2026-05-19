using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

// 数据结构：6DoF位姿+扳机值
[Serializable]
public class VRData
{
    // 头显数据
    public float h_x, h_y, h_z;
    public float h_rx, h_ry, h_rz, h_rw;

    // 左手柄
    public float l_x, l_y, l_z;
    public float l_rx, l_ry, l_rz, l_rw;
    public float l_trigger;

    // 右手柄
    public float r_x, r_y, r_z;
    public float r_rx, r_ry, r_rz, r_rw;
    public float r_trigger;
}

public class VRDataSender : MonoBehaviour
{
    [Header("UDP配置")]
    public string ip = "127.0.0.1";
    public int port = 8888;

    private UdpClient udp;
    private float timer = 0;
    private readonly float interval = 0.02f; // 50Hz

    // XR控制器引用（兼容Unity 2022的查找方式）
    private XRController leftController;
    private XRController rightController;
    private Transform headTransform;

    void Start()
    {
        // 初始化UDP客户端
        udp = new UdpClient();

        // 1. 找头显相机（XR Origin里的主相机）
        headTransform = Camera.main?.transform;

        // 2. 找左手柄控制器（直接按场景名称查找，兼容所有XR版本）
        GameObject leftHandObj = GameObject.Find("Left Controller");
        if (leftHandObj != null)
        {
            leftController = leftHandObj.GetComponent<XRController>();
        }

        // 3. 找右手柄控制器（直接按场景名称查找）
        GameObject rightHandObj = GameObject.Find("Right Controller");
        if (rightHandObj != null)
        {
            rightController = rightHandObj.GetComponent<XRController>();
        }

        Debug.Log("✅ 数据发送器启动成功！目标地址：" + ip + ":" + port);
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0;
            SendData();
        }
    }

    void SendData()
    {
        VRData data = new VRData();

        // 头显数据
        if (headTransform != null)
        {
            data.h_x = headTransform.position.x;
            data.h_y = headTransform.position.y;
            data.h_z = headTransform.position.z;
            data.h_rx = headTransform.rotation.x;
            data.h_ry = headTransform.rotation.y;
            data.h_rz = headTransform.rotation.z;
            data.h_rw = headTransform.rotation.w;
        }

        // 左手柄数据（位置+旋转+扳机）
        if (leftController != null && leftController.gameObject.activeSelf)
        {
            Transform leftHandTransform = leftController.transform;
            data.l_x = leftHandTransform.position.x;
            data.l_y = leftHandTransform.position.y;
            data.l_z = leftHandTransform.position.z;
            data.l_rx = leftHandTransform.rotation.x;
            data.l_ry = leftHandTransform.rotation.y;
            data.l_rz = leftHandTransform.rotation.z;
            data.l_rw = leftHandTransform.rotation.w;

            // 用XR官方API读扳机值（兼容新输入系统，无报错）
            if (leftController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float leftTrigger))
            {
                data.l_trigger = leftTrigger;
            }
        }

        // 右手柄数据（位置+旋转+扳机）
        if (rightController != null && rightController.gameObject.activeSelf)
        {
            Transform rightHandTransform = rightController.transform;
            data.r_x = rightHandTransform.position.x;
            data.r_y = rightHandTransform.position.y;
            data.r_z = rightHandTransform.position.z;
            data.r_rx = rightHandTransform.rotation.x;
            data.r_ry = rightHandTransform.rotation.y;
            data.r_rz = rightHandTransform.rotation.z;
            data.r_rw = rightHandTransform.rotation.w;

            // 用XR官方API读扳机值（兼容新输入系统，无报错）
            if (rightController.inputDevice.TryGetFeatureValue(CommonUsages.trigger, out float rightTrigger))
            {
                data.r_trigger = rightTrigger;
            }
        }

        // 转JSON字符串
        string json = JsonUtility.ToJson(data);
        Debug.Log("📤 发送数据：" + json);

        // UDP发送数据
        byte[] bytes = Encoding.UTF8.GetBytes(json);
        udp.Send(bytes, bytes.Length, ip, port);
    }

    void OnDestroy()
    {
        // 关键：先判断udp是否不为null，再Close，避免空引用报错
        if (udp != null)
        {
            udp.Close();
        }
    }
}