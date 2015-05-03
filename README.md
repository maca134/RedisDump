RedisDump
=========

This is a small tool to make it easy to backup and restore individual Redis DB's

Dumps and restores redis databases

        --file          Sets the backup/restore file. (Required)
        --host          Sets redis host. (Default: "localhost")
        --port          Sets redis port. (Default: 6379)
        --password      Sets redis password. (Default: "")
        --db            Sets redis DB ID  (Default: 0)
        --backup        Performs a backup 
        --restore       Performs a restore

Examples:

`redisdump.exe --db=1 --file="redis.dump" --backup --password=pass`
  
`redisdump.exe --file="redis.dump" --restore`
