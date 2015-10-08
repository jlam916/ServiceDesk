:: Clear routing tables
route -f
:: release IP address
ipconfig /release
:: Dynamic Host Configuration Protocol (DHCP) lease is renewed
ipconfig /renew
:: Address Resolution Protocol (ARP) cache is flushed
arp -d *
:: Reload of the NetBIOS name cache
nbtstat -R
:: NetBIOS name update is sent
nbtstat -RR
:: Domain Name System (DNS) cache is flushed
ipconfig /flushdns
:: DNS name registration
ipconfig /registerdns