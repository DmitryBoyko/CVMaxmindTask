# CVMaxmindTask
Update DB with Maxmind.com data and provide country name based on its IP address


Short description
Develop a web service to determine the user's geographic location by IP address.

Requirements:
1. The service must store geographic data for all IP addresses in its own database
running PostgreSQL.

2. The service should regularly update the specified database using the data of any
Provider (e.g. MaxMind GeoLite2: https://dev.maxmind.com/geoip/geoip2/geolite2/).

3. The service must have a REST API to obtain a geographical location (in
JSON format) at the specified IP address.

4. The task of updating the database should be implemented as a console
applications.

5. The REST API must be implemented on ASP.NET Core or ASP.NET Web API 2.

6. Both parts of the service must be written in C #.
