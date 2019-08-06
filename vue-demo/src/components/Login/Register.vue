<template>
  <div class="wrap">
    <el-form :model="form" class="container" ref="form" :rules="rules" status-icon>
      <el-form-item prop="userName">
        <el-input v-model="form.userName" auto-complete="off" placeholder="请输入账号" style="width:21%"></el-input>
      </el-form-item>
      <el-form-item prop="passWord">
        <el-input
          type="password"
          v-model="form.passWord"
          auto-complete="off"
          placeholder="设置密码"
          style="width:21%"
        ></el-input>
      </el-form-item>
      <el-form-item prop="checkPassword">
        <el-input
          type="password"
          v-model="form.checkPassword"
          auto-complete="off"
          placeholder="确认密码"
          style="width:21%"
        ></el-input>
      </el-form-item>
      <el-form-item prop="phone">
        <el-input
          v-model="form.phone"
          auto-complete="off"
          placeholder="请输入手机号"
          @change="checkPhone"
          style="width:21%"
        ></el-input>
      </el-form-item>
      <el-form-item prop="code">
        <el-input v-model="form.code" auto-complete="off" placeholder=" 请输入验证码" style="width:14%"></el-input>
        <el-button
          @click="getCode()"
          v-if="btnTitle"
          :disabled="disabled"
          style="width:7%"
        >{{btnTitle}}</el-button>
      </el-form-item>
      <el-form-item>
        <el-button @click="register('form')" style="width:21%">立即注册</el-button>
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
import { validatePhone, validateIdPassWord } from "./validate";
export default {
  /** 组件名称 */
  name: "Register",
  /** 绑定数据 */
  data() {
    /*验证账号是否英文数字以及下划线*/
    var validateIdName = (rule, value, callback) => {
      const reg = /^[_a-zA-Z0-9]+$/;
      if (value.length < 6) {
        console.log(callback);
        callback(new Error("账号不得小于6位数"));
      } else if (!reg.test(value)) {
        callback(new Error("账号为英文字母，数字以或下划线组成"));
      } else if (reg.test(value)) {
        this.isExistName(callback);
      } else {
        callback();
      }
    };
    /** 二次密码检测 */
    var checkPassword = (rule, value, callback) => {
      if (value === "") {
        callback(new Error("请再次输入密码"));
      } else if (value !== this.form.passWord) {
        callback(new Error("两次输入密码不一致"));
      } else {
        callback();
      }
    };
    /** 验证码检测 */
    var checkCode = (rule, value, callback) => {
      let code = sessionStorage.getItem("code");
      if (value === "") {
      } else if (value !== code) {
        callback(new Error("验证码错误"));
      } else {
        callback();
      }
    };
    return {
      /** 表单数据 */
      form: {
        userName: "",
        passWord: "",
        checkPassword: "",
        phone: "",
        code: ""
      },
      mobile_code: "",
      disabled: true,
      btnTitle: "获取验证码",
      rules: {
        userName: [{ validator: validateIdName, trigger: "blur" }],
        passWord: [{ validator: validateIdPassWord, trigger: "blur" }],
        checkPassword: [{ validator: checkPassword, trigger: "blur" }],
        phone: [{ validator: validatePhone, trigger: "blur" }],
        code: [{ validator: checkCode, trigger: "blur" }]
      }
    };
  },
  /** 回车注册事件 */
  created() {
    var _this = this;
    document.onkeydown = function(e) {
      var key = window.event.keyCode;
      if (key == 13) {
        _this.register("form");
      }
    };
  },
  methods: {
    isExistName(callback) {
      var url = "http://localhost:10000/api/Login";
      this.$http
        .get(url, { params: { id: this.form.userName } })
        .then(response => {
          console.log(response);
          if (response.data) {
            callback(new Error("账号已存在"));
          } else {
            callback();
          }
        });
    },
    /** 号码正确验证按钮可使用 */
    checkPhone() {
      const reg = /^((0\d{2,3}-\d{7,8})|(1[34578]\d{9}))$/;
      if (reg.test(this.form.phone)) {
        this.disabled = false;
      } else this.disabled = true;
    },
    /** 获得验证码 */
    getCode() {
      window.sessionStorage.removeItem("code");
      this.validateBtn();
      var url = "http://localhost:10000/api/Login/id";
      this.$http
        .post(
          url,
          Qs.stringify({
            data: this.form.phone
          }),
          {
            headers: {
              "Content-type": "application/x-www-form-urlencoded"
            }
          }
        )
        .then(response => {
          window.sessionStorage.setItem("code", response.data);
        });
    },
    /** 验证码倒计时 */
    validateBtn() {
      //倒计时
      let time = 60;
      let timer = setInterval(() => {
        if (time == 0) {
          clearInterval(timer);
          this.disabled = false;
          this.btnTitle = "重新获取";
        } else {
          this.btnTitle = time + "秒后重试";
          this.disabled = true;
          time--;
        }
      }, 1000);
    },
    /** 注册 */
    register(form) {
      var url = "http://localhost:10000/api/Login/data";
      this.$refs[form].validate(valid => {
        if (valid) {
          this.$http
            .post(url, Qs.stringify({ data: this.form }), {
              headers: {
                "Content-type": "application/x-www-form-urlencoded"
              }
            })
            .then(response => {
              if (response.status == "200") {
                this.$message({
                  message: "恭喜你，账号注册成功！",
                  type: "success"
                });
                //自动跳转
                let time = 1;
                let timer = setInterval(() => {
                  if (time == 0) {
                    this.$router.push({
                      path: "/Home"
                    });
                  } else {
                    time--;
                  }
                }, 1000);
              }
            });
        } else {
          console.log("error submit!!");
          return false;
        }
      });
    }
  }
};
</script>
<style>
.el-form-item__error {
  margin-left: 40%;
  color: red;
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
  margin-top: 15%;
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
