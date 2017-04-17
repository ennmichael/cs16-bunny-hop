#include "stdafx.h"
#include <windows.h> 
#include <tlhelp32.h> 
#include <tchar.h>
#include <memory>
#include <string>

namespace ModuleLookup {
	constexpr DWORD snapshotFlags = TH32CS_SNAPMODULE | TH32CS_SNAPMODULE32;

	struct HandleDeleter {
		void operator()(HANDLE handle) const noexcept {
			CloseHandle(handle);
		}

		using pointer = HANDLE;
	};

	using UniqueHandle = std::unique_ptr<HANDLE, HandleDeleter>;

	UniqueHandle CreateUniqueToolhelp32Snapshot(DWORD flags, DWORD processId) {
		return UniqueHandle(
			CreateToolhelp32Snapshot(flags, processId)
		);
	}

	bool handleIsValid(const HANDLE handle) noexcept {
		return handle != INVALID_HANDLE_VALUE;
	}

	MODULEENTRY32 CreateModuleEntry32() {
		MODULEENTRY32 moduleEntry;
		moduleEntry.dwSize = sizeof(moduleEntry);
		return moduleEntry;
	}

	MODULEENTRY32 FindModuleInModuleList(const HANDLE snapshot, const std::wstring& moduleName) noexcept {
		auto result = CreateModuleEntry32();
		if (Module32First(snapshot, &result))
		{
			do
			{
				if (moduleName == result.szModule)
					return result;
			} while (Module32Next(snapshot, &result));
		}
		return result;
	}

	DWORD_PTR GetModuleBaseAddressImpl(DWORD processId, const std::wstring& moduleName) noexcept {
		auto snapshot = CreateUniqueToolhelp32Snapshot(snapshotFlags, processId);
		if (!handleIsValid(snapshot.get()))
			return 0;
		auto moduleEntry = FindModuleInModuleList(snapshot.get(), moduleName);
		return reinterpret_cast<DWORD_PTR>(moduleEntry.modBaseAddr);
	}

	extern "C" {
		__declspec(dllexport)
		DWORD_PTR GetModuleBaseAddress(DWORD processId, const TCHAR* moduleName) {
			return GetModuleBaseAddressImpl(processId, moduleName);
		}

		__declspec(dllexport)
		TCHAR* FormatErrorMessage(DWORD ErrorCode) // LET IT DANGLE
		{
			TCHAR   *pMsgBuf = NULL;
			DWORD   nMsgLen = FormatMessage(FORMAT_MESSAGE_ALLOCATE_BUFFER |
				FORMAT_MESSAGE_FROM_SYSTEM | FORMAT_MESSAGE_IGNORE_INSERTS,
				NULL, ErrorCode, MAKELANGID(LANG_NEUTRAL, SUBLANG_DEFAULT),
				reinterpret_cast<LPTSTR>(&pMsgBuf), 0, NULL);
			if (!nMsgLen)
				return _T("FormatMessage fail");
			return pMsgBuf;
		}
	}
}
