@ECHO OFF
 
cd C:\Users\bvpmi\Documents\Publishings\ClearSpam\
del appsettings.json
del appsettings.zenBookFlip.json
del web.release.config

docker build -t clearspam.web ./bin/release/netcoreapp2.2/publish

docker tag clearspam.web registry.heroku.com/clearspam/web

docker push registry.heroku.com/clearspam/web

heroku container:release web -a clearspam

PAUSE