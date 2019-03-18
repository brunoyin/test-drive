package db;

public class College{
    String Id, Name, City, State, Zip;
    int Region;
    Double Latitude, Longitude,  AdmRate, SatAvg, ActAvg, Earnings, Cost, Enrollments;

    
    public String getId() {
        return Id;
    }
    public void setId(String id) {
        Id = id;
    }
    public String getName() {
        return Name;
    }
    public void setName(String name) {
        Name = name;
    }
    public String getCity() {
        return City;
    }
    public void setCity(String city) {
        City = city;
    }
    public String getState() {
        return State;
    }
    public void setState(String state) {
        State = state;
    }
    public String getZip() {
        return Zip;
    }
    public void setZip(String zip) {
        Zip = zip;
    }
    public int getRegion() {
        return Region;
    }
    public void setRegion(int region) {
        Region = region;
    }
    public Double getLatitude() {
        return Latitude;
    }
    public void setLatitude(Double latitude) {
        Latitude = latitude;
    }
    public Double getLongitude() {
        return Longitude;
    }
    public void setLongitude(Double longitude) {
        Longitude = longitude;
    }
    public Double getAdmRate() {
        return AdmRate;
    }
    public void setAdmRate(Double admRate) {
        AdmRate = admRate;
    }
    public Double getSatAvg() {
        return SatAvg;
    }
    public void setSatAvg(Double satAvg) {
        SatAvg = satAvg;
    }
    public Double getActAvg() {
        return ActAvg;
    }
    public void setActAvg(Double actAvg) {
        ActAvg = actAvg;
    }
    public Double getEarnings() {
        return Earnings;
    }
    public void setEarnings(Double earnings) {
        Earnings = earnings;
    }
    public Double getCost() {
        return Cost;
    }
    public void setCost(Double cost) {
        Cost = cost;
    }
    public Double getEnrollments() {
        return Enrollments;
    }
    public void setEnrollments(Double enrollments) {
        Enrollments = enrollments;
    }

}