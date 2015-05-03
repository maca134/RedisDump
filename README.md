RedisDump
=========

This is a small tool to make it easy to backup and restore individual Redis DB's

Dumps and restores redis databases

        --file          Sets the backup/restore file.
        --host          Sets redis host.
        --port          Sets redis port.
        --password      Sets redis password.
        --db            Sets redis DB ID
        --backup        Performs a backup
        --restore       Performs a restore

Examples:

`redisdump.exe --db=1 --file="redis.dump" --backup --password=pass`
  
`redisdump.exe --file="redis.dump" --restore`
