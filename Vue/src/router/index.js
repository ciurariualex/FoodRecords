// Vue imports
import Vue from 'vue'
import Router from 'vue-router'

// 3rd party imports
import Auth from '@okta/okta-vue'

Vue.use(Auth, {
  issuer: 'https://dev-589054.okta.com',
  client_id: '0oat3rr41qXZZiRnW4x6',
  redirect_uri: 'http://localhost:8080/implicit/callback',
  scope: 'openid profile email'
})

Vue.use(Router)

let router = new Router({
  mode: 'history',
  routes: [
	{
  	path: '/implicit/callback',
  	component: Auth.handleCallback()
  }
  ]
})

router.beforeEach(Vue.prototype.$auth.authRedirectGuard())

export default router