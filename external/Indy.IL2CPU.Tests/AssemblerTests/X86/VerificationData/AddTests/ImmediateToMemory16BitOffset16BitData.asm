use32
org 0x200000
			add word [EAX + 0x4B3], 0x41
			add word [EBX + 0x4B3], 0x41
			add word [ECX + 0x4B3], 0x41
			add word [EDX + 0x4B3], 0x41
			add word [ESI + 0x4B3], 0x41
			add word [EDI + 0x4B3], 0x41
			add word [EBP + 0x4B3], 0x41
			add word [ESP + 0x4B3], 0x41
