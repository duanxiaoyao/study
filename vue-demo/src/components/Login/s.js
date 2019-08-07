import Qs from "qs";
import axios from "axios";

export function post(url, data, callback) {
    var url = "http://localhost:1111/api/"+url;
    axios({
        method: 'post',
        url: url,
        params:Qs.stringify({
            data
        }),
            headers: {
              "Content-type": "application/x-www-form-urlencoded"
            }
    }).then(function(respose) {
            callback(respose.data)
          })
          .then(function() {})
          .catch(function(respose) {})
  }