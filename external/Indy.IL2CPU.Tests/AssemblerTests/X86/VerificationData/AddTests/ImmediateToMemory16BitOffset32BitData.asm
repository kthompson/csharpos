use32
org 0x200000
			add dword [EAX + 0x4B3], 0x41
			add dword [EBX + 0x4B3], 0x41
			add dword [ECX + 0x4B3], 0x41
			add dword [EDX + 0x4B3], 0x41
			add dword [ESI + 0x4B3], 0x41
			add dword [EDI + 0x4B3], 0x41
			add dword [EBP + 0x4B3], 0x41
			add dword [ESP + 0x4B3], 0x41