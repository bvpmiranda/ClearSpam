@ECHO OFF

cd C:\Users\bvpmi\Documents\Publishings\ClearSpam\
del appsettings.json
del appsettings.zenBookFlip.json
copy appsettings.release.json appsettings.json
del web.release.config

docker build -t clearspam.web ./

docker tag clearspam.web registry.heroku.com/clearspam/web

docker push registry.heroku.com/clearspam/web

heroku container:release web -a clearspam

PAUSE