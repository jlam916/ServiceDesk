:: Remapp users drives
IF NOT Exist S: net use S: \\IWMDOCS\IWM
IF NOT Exist J: net use J: \\IWMDOCS\APPSPOOL
IF NOT Exist P: net use P: \\IWMDOCS\Private\%username%