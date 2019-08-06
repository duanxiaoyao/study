<template>
  <div class="wrap">
    <el-form :model="form" class="container" ref="form" :rules="rules">
      <el-form-item prop="userName">
        <!-- <i class="el-icon-user" style="font-size:30px"></i> -->
        <el-input v-model="form.userName" placeholder="账号" style="width:21%"></el-input>
      </el-form-item>
      <el-form-item v-if="visible" prop="passWord">
        <!-- <i class="el-icon-lock" style="font-size:30px"></i> -->
        <el-input type="password" v-model="form.passWord" placeholder="密码" style="width:21%"></el-input>
        <span class="show-pwd" @click="showPwd('show')">
          <svg-icon icon-class="eye"/>
        </span>
      </el-form-item>
      <el-form-item v-else prop="passWord">
        <!-- <i class="el-icon-lock" style="font-size:30px"></i> -->
        <el-input type="text" v-model="form.passWord" placeholder="密码" style="width:21%"></el-input>
        <span class="show-pwd" @click="showPwd('hide')">
          <svg-icon icon-class="eye-open"/>
        </span>
      </el-form-item>
      <el-form-item>
        <el-button @click="login('form')" style="width:10%">登陆</el-button>
        <el-button @click="register()" style="width:10%">注册</el-button>
      </el-form-item>
    </el-form>
    <ul>
      <li></li>
      <li></li>
      <li></li>
      <li></li>
      <li></li>
      <li></li>
      <li></li>
      <li></li>
      <li></li>
      <li></li>
    </ul>
  </div>
</template>

<script>
import Qs from "qs";
import axios from "axios";
export default {
  /** 组件名称 */
  name: "Login",
  /** 绑定数据 */
  data() {
    var validateIdName = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入账号"));
      } else {
        callback();
      }
    };
    var validateIdPassWord = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请输入密码"));
      } else {
        callback();
      }
    };
    return {
      /** 表单数据 */
      form: {
        userName: "",
        passWord: ""
      },
      rules: {
        userName: [{ validator: validateIdName, trigger: "blur" }],
        passWord: [{ validator: validateIdPassWord, trigger: "blur" }]
      },
      visible: true
    };
  },
  /** 回车事件 */
  created() {
    var _this = this;
    document.onkeydown = function(e) {
      var key = window.event.keyCode;
      if (key == 13) {
        _this.login("form");
      }
    };
  },
  methods: {
    /** 显示密码 */
    showPwd(value) {
      this.visible = !(value === "show");
    },
    /** 登陆 */
    login(form) {
      var url = "http://localhost:10000/api/Login";
      this.$refs[form].validate(valid => {
        if (valid) {
          this.$http
            .post(
              url,
              Qs.stringify({
                userName: this.form.userName,
                passWord: this.form.passWord
              }),
              {
                headers: {
                  "Content-type": "application/x-www-form-urlencoded"
                }
              }
            )
            .then(response => {
              if(response.data == true)
              {
                this.$message({
                  message: "欢迎来到德莱联盟！",
                  type: "success"
                });
                this.$router.push({
                  path:'/Home'
                })
              }
              else
              {
                this.$message({
                  message: "账号或密码错误！",
                  type: "info"
                });
              }
            });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    },
    /** 注册 */
    register() {
      let url = this.$router.resolve({ path: "/Register" });
      window.open(url.href, "_blank");
    }
  }
};
</script>
<style>
.el-form-item__error {
  margin-left: 40%;
  color: red;
}
.show-pwd {
  position: absolute;
  right: 40%;
  font-size: 16px;
  cursor: pointer;
  user-select: none;
}
.wrap {
  width: 100%;
  height: 100%;
  position: fixed;
  opacity: 0.8;
  background: linear-gradient(to bottom right, #50a3a2, #53e3a6);
  background: -webkit-linear-gradient(to bottom right, #50a3a2, #53e3a6);
}
.container {
  text-align: center;
  margin-top: 20%;
}
.wrap ul {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  z-index: -10;
}
.wrap ul li {
  list-style-type: none;
  display: block;
  position: absolute;
  bottom: -120px;
  width: 15px;
  height: 15px;
  z-index: -8;
  background-color: rgba(255, 255, 255, 0.15);
  animotion: square 25s infinite;
  -webkit-animation: square 25s infinite;
}
.wrap ul li:nth-child(1) {
  width: 60px;
  height: 60px;
  left: 0;
  animation-duration: 10s;
  -moz-animation-duration: 10s;
  -o-animation-duration: 10s;
  -webkit-animation-duration: 10s;
}
.wrap ul li:nth-child(2) {
  width: 40px;
  height: 40px;
  left: 10%;
  animation-duration: 15s;
  -moz-animation-duration: 15s;
  -o-animation-duration: 15s;
  -webkit-animation-duration: 15s;
}
.wrap ul li:nth-child(3) {
  left: 20%;
  width: 25px;
  height: 25px;
  animation-duration: 12s;
  -moz-animation-duration: 12s;
  -o-animation-duration: 12s;
  -webkit-animation-duration: 12s;
}
.wrap ul li:nth-child(4) {
  width: 50px;
  height: 50px;
  left: 30%;
  -webkit-animation-delay: 3s;
  -moz-animation-delay: 3s;
  -o-animation-delay: 3s;
  animation-delay: 3s;
  animation-duration: 12s;
  -moz-animation-duration: 12s;
  -o-animation-duration: 12s;
  -webkit-animation-duration: 12s;
}
.wrap ul li:nth-child(5) {
  width: 60px;
  height: 60px;
  left: 40%;
  animation-duration: 8s;
  -moz-animation-duration: 8s;
  -o-animation-duration: 8s;
  -webkit-animation-duration: 8s;
}
.wrap ul li:nth-child(6) {
  width: 75px;
  height: 75px;
  left: 50%;
  -webkit-animation-delay: 15s;
  -moz-animation-delay: 15s;
  -o-animation-delay: 15s;
  animation-delay: 15s;
}
.wrap ul li:nth-child(7) {
  width: 40px;
  height: 40px;
  left: 60%;
  animation-duration: 12s;
  -moz-animation-duration: 12s;
  -o-animation-duration: 12s;
  -webkit-animation-duration: 12s;
}
.wrap ul li:nth-child(8) {
  width: 90px;
  height: 90px;
  left: 70%;
  -webkit-animation-delay: 4s;
  -moz-animation-delay: 4s;
  -o-animation-delay: 4s;
  animation-delay: 4s;
}
.wrap ul li:nth-child(9) {
  width: 60px;
  height: 60px;
  left: 80%;
  animation-duration: 15s;
  -moz-animation-duration: 15s;
  -o-animation-duration: 15s;
  -webkit-animation-duration: 15s;
}
.wrap ul li:nth-child(10) {
  width: 100px;
  height: 100px;
  left: 90%;
  -webkit-animation-delay: 6s;
  -moz-animation-delay: 6s;
  -o-animation-delay: 6s;
  animation-delay: 6s;
  animation-duration: 20s;
  -moz-animation-duration: 20s;
  -o-animation-duration: 20s;
  -webkit-animation-duration: 20s;
}
@keyframes square {
  0% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
  }
  100% {
    bottom: 800px;
    transform: rotate(600deg);
    -webit-transform: rotate(600deg);
    -webkit-transform: translateY(-500);
    transform: translateY(-500);
  }
}
@-webkit-keyframes square {
  0% {
    -webkit-transform: translateY(0);
    transform: translateY(0);
  }
  100% {
    bottom: 800px;
    transform: rotate(600deg);
    -webit-transform: rotate(600deg);
    -webkit-transform: translateY(-500);
    transform: translateY(-500);
  }
}
</style>
