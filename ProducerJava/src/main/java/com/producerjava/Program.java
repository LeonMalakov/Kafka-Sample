package com.producerjava;

import org.apache.kafka.clients.producer.KafkaProducer;
import org.apache.kafka.clients.producer.ProducerConfig;
import org.apache.kafka.clients.producer.ProducerRecord;
import org.apache.kafka.common.serialization.StringSerializer;

import java.util.Properties;

public class Program {
    public static void main(String[] args) {
        System.out.println("Started");

        String kafkaHosts = System.getenv(Constants.HOSTS_ENV);

        if(kafkaHosts == null || kafkaHosts.isEmpty()) {
            System.out.println("Kafka hosts not found");
            return;
        }

        Properties properties = new Properties();
        properties.setProperty(ProducerConfig.BOOTSTRAP_SERVERS_CONFIG, kafkaHosts);
        properties.setProperty(ProducerConfig.KEY_SERIALIZER_CLASS_CONFIG, StringSerializer.class.getName());
        properties.setProperty(ProducerConfig.VALUE_SERIALIZER_CLASS_CONFIG, StringSerializer.class.getName());

        try(KafkaProducer<String, String> producer = new KafkaProducer<String, String>(properties)) {
            for(int i = 0; i < 5; i++){
                ProducerRecord<String, String> record = new ProducerRecord<String, String>(Constants.TOPIC, "Item " + i);
                producer.send(record);
                System.out.println(record.value());
            }
        } catch (Exception e) {
            System.out.println(e.getMessage());
        }

        System.out.println("Shutdown");
    }
}
