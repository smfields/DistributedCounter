FROM ubuntu AS base
EXPOSE 80

# Download ghz-web
RUN apt-get -y update; apt-get -y install curl; 
RUN curl -L -o ghz-linux-x86_64.tar.gz https://github.com/bojand/ghz/releases/download/v0.117.0/ghz-linux-x86_64.tar.gz
RUN tar -xvzf  ghz-linux-x86_64.tar.gz

# Copy config
COPY config.toml .

# Run web
CMD ["./ghz-web"]