#!/bin/sh

# echo Building SFIT Discovery Track Suggestion Microservice Docker Image
echo Running SFIT Discovery Track Suggestion Microservice

# set -o allexport
# source ./.env.production
# set +o allexport

# docker-compose config
# docker-compose up

# docker build --no-cache -f "./Dockerfile" --force-rm -t sfit-ml-track-suggestion-api:0.0.1 --target final  --label "com.microsoft.created-by=visual-studio" --label "com.microsoft.visual-studio.project-name=MulticlassClassification-TrackClassifier.TrackClassifier.App" .
docker run -it --rm -p 8080:80 -p 4443:443 --name sfit-ml-track-suggestion-api sfit-ml-track-suggestion-api:0.0.1

read -n 1 -s -r -p "Press any key to continue"