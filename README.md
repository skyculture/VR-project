# VR Data Sender

Unity VR项目，用于从头显/手柄提取6DoF空间位姿和扳机键数据，并通过UDP局域网发送。

## 功能特性

- 提取头显6DoF数据（XYZ坐标 + 四元数旋转）
- 提取左手柄6DoF数据 + 扳机值（0.0~1.0）
- 提取右手柄6DoF数据 + 扳机值（0.0~1.0）
- JSON格式数据打包
- 50Hz发送频率
- 支持XR Device Simulator模拟测试

## 快速开始

### 环境要求
- Unity 2022/2023
- XR Interaction Toolkit
- XR Device Simulator

### 使用步骤

1. 克隆或下载项目
2. 在Unity中打开项目
3. 打开 `Assets/Scenes/SampleScene.unity`
4. 在场景中添加一个空GameObject，挂上 `VRDataSender.cs` 脚本
5. （可选）在同一或另一个GameObject上挂上 `UDPReceiver.cs` 脚本用于本地测试
6. 点击Play运行

### 数据格式

发送的JSON数据格式：

```json
{
  "h_x": 0.0,
  "h_y": 0.0,
  "h_z": 0.0,
  "h_rx": 0.0,
  "h_ry": 0.0,
  "h_rz": 0.0,
  "h_rw": 1.0,
  "l_x": 0.0,
  "l_y": 0.0,
  "l_z": 0.0,
  "l_rx": 0.0,
  "l_ry": 0.0,
  "l_rz": 0.0,
  "l_rw": 1.0,
  "l_trigger": 0.0,
  "r_x": 0.0,
  "r_y": 0.0,
  "r_z": 0.0,
  "r_rx": 0.0,
  "r_ry": 0.0,
  "r_rz": 0.0,
  "r_rw": 1.0,
  "r_trigger": 0.0
}
```

### 配置说明

在 `VRDataSender` 脚本的Inspector面板中可以配置：
- `IP`: 目标IP地址（默认127.0.0.1）
- `Port`: 目标端口（默认8888）

## 脚本说明

- `VRDataSender.cs`: 数据发送脚本，负责提取VR数据并通过UDP发送
- `UDPReceiver.cs`: 数据接收脚本，用于本地测试验证

## 第一周任务完成情况

✅ 建工程：搭建包含 Meta SDK 的基础 Unity 空场景
✅ 获取数据：实时打印右/左手柄的世界坐标和旋转，以及扳机键值
✅ 封包发送：JSON格式打包，50Hz频率UDP发送
✅ 本地自测：UDP接收端测试
