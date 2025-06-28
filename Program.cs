using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Networking.NetworkOperators;

namespace HotSpotSwitch;

internal class Program
{
	public static async Task Main(string[] args)
	{
		var conProfile = NetworkInformation.GetInternetConnectionProfile();
		var tetheringManager = NetworkOperatorTetheringManager.CreateFromConnectionProfile(conProfile);
		var curstatus = tetheringManager.TetheringOperationalState;
		if (curstatus == TetheringOperationalState.On)
		{
			Console.WriteLine("移动热点当前已启用，正在关闭...");
			var result = await tetheringManager.StopTetheringAsync();
			if (result.Status == TetheringOperationStatus.Success)
			{
				Console.WriteLine("操作成功");
			}
			else
			{
				Console.WriteLine("操作失败:" + result.AdditionalErrorMessage);
			}
		}
		else if (curstatus == TetheringOperationalState.Off)
		{
			Console.WriteLine("移动热点当前未启用，正在启动...");
			var apConfig = tetheringManager.GetCurrentAccessPointConfiguration();
			apConfig.Ssid = "kingnb";
			apConfig.Passphrase = "12345687";
			await tetheringManager.ConfigureAccessPointAsync(apConfig);
			var result = await tetheringManager.StartTetheringAsync();
			if (result.Status == TetheringOperationStatus.Success)
			{
				Console.WriteLine("操作成功");
			}
			else
			{
				Console.WriteLine("操作失败:" + result.AdditionalErrorMessage);
			}
		}
		else if (curstatus == TetheringOperationalState.InTransition)
		{
			Console.WriteLine("正在处理中...");
		}
		else
		{
			Console.WriteLine("未知状态...");
		}

		Task.Delay(1000).Wait();
	}
}