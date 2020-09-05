<template>
  <div id="app" :class="typeof weather.condition != 'undefined'&& weather.temp > 70 ? 'warm': ''">
   <main> 
     <div class="search-box">
       <input type="text" class="search-bar" 
       placeholder="Search..." v-model="query" @keypress="fetchWeather"/>

       
     </div>

     <div class="weather-wrap" v-if="typeof weather.condition != 'undefined'">
       <div class="location-box">
         <div class="location"> {{ weather.cityName }}, {{ weather.country}} </div>
         <div class="date"> {{ dateBuilder() }} </div>
       </div>
       <div class="weather-box">
         <div class="temp"> {{weather.temp}} </div>
         <div class="weather"> {{weather.condition}} </div>
       </div>

     </div>

   </main> 
  </div>
</template>

<script>

export default {
  name: 'app',
  data () {
    return {
      query:'',
      weather:{}
    }
  },

  methods:{
    fetchWeather (e){
      if (e.key == "Enter"){
        fetch(`/api/GetWeather?query=${this.query}`)
        .then(res => {
          return res.json();
        }).then(this.setResults);
      }
    },
    setResults (results) {
      this.weather = results;
    },
    dateBuilder () {
      let d = new Date();
      let months = ["January", "February", "March", 
      "April","May", "June","July", "August", "September", "October", "November", "December"];
      let days = ["Sunday","Monday", "Tuesday", "Wednesday", "Thrusday", "Friday", "Saturday"];
      let day = days[d.getDay()];
      let date = d.getDate();
      let month = months[d.getMonth()];
      let year = d.getFullYear();
      return`${day} ${date} ${month} ${year}`;
    },
    

  }

  
}
</script>

<style>
* {
  margin: 0;
  padding: 0;
  box-sizing: border-box;
}

body {
  font-family: 'montserrat', sans-serif;
}

#app{
  background-image: linear-gradient(to bottom, rgba(12, 132, 245, 0.25), rgba(0, 26, 255, 0.75));
  background-size: cover;
  background-position: bottom;
  transition: 0.4s;
}

#app.warm{
  background-image: linear-gradient(to bottom, rgba(255, 136, 0, 0.931), rgba(246, 78, 0, 0.75));
}

main {
  min-height: 100vh;
  padding: 25px;
  background-image: linear-gradient(to bottom, rgba(0,0,0,0.25), rgba(0,0,0,0.75));
}

.search-box{
  width: 100%;
  margin-bottom: 30px;
  
}

.search-box .search-bar {
  display: block;
  width: 100%;
  padding: 15px;

  appearance: none;
  border: none;
  outline: none;
  background:none;

 
  background-color: rgba(255,255,255,0.5);
  border-radius: 10px 10px 10px 10px;

}

.search-box .search-bar:focus{
  box-shadow:0px 0px 16px rgba(0,0,0,0.25) ;
  background-color: rgba(255,255,255,0.75);
  border-radius: 10px 10px 10px 10px;

}

.location-box .location{
  color: #ffffff;
  font-size: 32px;
  font-weight: 500;
  text-align: center;
  text-shadow: 1px 3px rgba(0,0,0,0.25);

}

.location-box .date{

  color: #ffffff;
  font-size: 20px;
  font-weight: 300;
  text-align: center;
  font-style: italic;

}

.weather-box{
  text-align: center;
}

.weather-box .temp{
  display: inline-block;
  padding: 10px 25px;
  color: #ffffff;
  font-size: 102px;
  font-weight: 900;
}

.weather-box .weather{
  color: #ffffff;
  font-size: 48px;
  font-weight: 700;
  font-style: italic;
  text-shadow: 3px 6px rgba(0,0,0,0.25);

}

</style>
