"use strict";(self.webpackChunkorchardcore_apis_graphql=self.webpackChunkorchardcore_apis_graphql||[]).push([[724],{7181:(e,t,n)=>{n.d(t,{a:()=>c,b:()=>f,c:()=>d,d:()=>m,e:()=>g,g:()=>l});var i=n(275),a=n(2584),r=n(1520),o=Object.defineProperty,u=(e,t)=>o(e,"name",{value:t,configurable:!0});function l(e,t){const n={schema:e,type:null,parentType:null,inputType:null,directiveDef:null,fieldDef:null,argDef:null,argDefs:null,objectFieldDefs:null};return(0,r.f)(t,(t=>{var a,r;switch(t.kind){case"Query":case"ShortQuery":n.type=e.getQueryType();break;case"Mutation":n.type=e.getMutationType();break;case"Subscription":n.type=e.getSubscriptionType();break;case"InlineFragment":case"FragmentDefinition":t.type&&(n.type=e.getType(t.type));break;case"Field":case"AliasedField":n.fieldDef=n.type&&t.name?s(e,n.parentType,t.name):null,n.type=null===(a=n.fieldDef)||void 0===a?void 0:a.type;break;case"SelectionSet":n.parentType=n.type?(0,i.xC)(n.type):null;break;case"Directive":n.directiveDef=t.name?e.getDirective(t.name):null;break;case"Arguments":const o=t.prevState?"Field"===t.prevState.kind?n.fieldDef:"Directive"===t.prevState.kind?n.directiveDef:"AliasedField"===t.prevState.kind?t.prevState.name&&s(e,n.parentType,t.prevState.name):null:null;n.argDefs=o?o.args:null;break;case"Argument":if(n.argDef=null,n.argDefs)for(let e=0;e<n.argDefs.length;e++)if(n.argDefs[e].name===t.name){n.argDef=n.argDefs[e];break}n.inputType=null===(r=n.argDef)||void 0===r?void 0:r.type;break;case"EnumValue":const u=n.inputType?(0,i.xC)(n.inputType):null;n.enumValue=u instanceof i.mR?p(u.getValues(),(e=>e.value===t.name)):null;break;case"ListValue":const l=n.inputType?(0,i.tf)(n.inputType):null;n.inputType=l instanceof i.p2?l.ofType:null;break;case"ObjectValue":const c=n.inputType?(0,i.xC)(n.inputType):null;n.objectFieldDefs=c instanceof i.sR?c.getFields():null;break;case"ObjectField":const f=t.name&&n.objectFieldDefs?n.objectFieldDefs[t.name]:null;n.inputType=null==f?void 0:f.type;break;case"NamedType":n.type=t.name?e.getType(t.name):null}})),n}function s(e,t,n){return n===a.S.name&&e.getQueryType()===t?a.S:n===a.T.name&&e.getQueryType()===t?a.T:n===a.a.name&&(0,i.Gv)(t)?a.a:t&&t.getFields?t.getFields()[n]:void 0}function p(e,t){for(let n=0;n<e.length;n++)if(t(e[n]))return e[n]}function c(e){return{kind:"Field",schema:e.schema,field:e.fieldDef,type:y(e.fieldDef)?null:e.parentType}}function f(e){return{kind:"Directive",schema:e.schema,directive:e.directiveDef}}function d(e){return e.directiveDef?{kind:"Argument",schema:e.schema,argument:e.argDef,directive:e.directiveDef}:{kind:"Argument",schema:e.schema,argument:e.argDef,field:e.fieldDef,type:y(e.fieldDef)?null:e.parentType}}function m(e){return{kind:"EnumValue",value:e.enumValue||void 0,type:e.inputType?(0,i.xC)(e.inputType):void 0}}function g(e,t){return{kind:"Type",schema:e.schema,type:t||e.type}}function y(e){return"__"===e.name.slice(0,2)}u(l,"getTypeInfo"),u(s,"getFieldDef"),u(p,"find"),u(c,"getFieldReference"),u(f,"getDirectiveReference"),u(d,"getArgumentReference"),u(m,"getEnumValueReference"),u(g,"getTypeReference"),u(y,"isMetaField")},1520:(e,t,n)=>{function i(e,t){const n=[];let i=e;for(;null==i?void 0:i.kind;)n.push(i),i=i.prevState;for(let e=n.length-1;e>=0;e--)t(n[e])}n.d(t,{f:()=>i}),(0,Object.defineProperty)(i,"name",{value:"forEachState",configurable:!0})},724:(e,t,n)=>{n.r(t);var i=n(7480),a=n(7181),r=(n(9361),n(7294),n(3935),n(2584),n(1520),Object.defineProperty),o=(e,t)=>r(e,"name",{value:t,configurable:!0});function u(e,t){const n=t.target||t.srcElement;if(!(n instanceof HTMLElement))return;if("SPAN"!==(null==n?void 0:n.nodeName))return;const i=n.getBoundingClientRect(),a={left:(i.left+i.right)/2,top:(i.top+i.bottom)/2};e.state.jump.cursor=a,e.state.jump.isHoldingModifier&&f(e)}function l(e){e.state.jump.isHoldingModifier||!e.state.jump.cursor?e.state.jump.isHoldingModifier&&e.state.jump.marker&&d(e):e.state.jump.cursor=null}function s(e,t){if(e.state.jump.isHoldingModifier||!c(t.key))return;e.state.jump.isHoldingModifier=!0,e.state.jump.cursor&&f(e);const n=o((o=>{o.code===t.code&&(e.state.jump.isHoldingModifier=!1,e.state.jump.marker&&d(e),i.C.off(document,"keyup",n),i.C.off(document,"click",a),e.off("mousedown",r))}),"onKeyUp"),a=o((t=>{const n=e.state.jump.destination;n&&e.state.jump.options.onClick(n,t)}),"onClick"),r=o(((t,n)=>{e.state.jump.destination&&(n.codemirrorIgnore=!0)}),"onMouseDown");i.C.on(document,"keyup",n),i.C.on(document,"click",a),e.on("mousedown",r)}i.C.defineOption("jump",!1,((e,t,n)=>{if(n&&n!==i.C.Init){const t=e.state.jump.onMouseOver;i.C.off(e.getWrapperElement(),"mouseover",t);const n=e.state.jump.onMouseOut;i.C.off(e.getWrapperElement(),"mouseout",n),i.C.off(document,"keydown",e.state.jump.onKeyDown),delete e.state.jump}if(t){const n=e.state.jump={options:t,onMouseOver:u.bind(null,e),onMouseOut:l.bind(null,e),onKeyDown:s.bind(null,e)};i.C.on(e.getWrapperElement(),"mouseover",n.onMouseOver),i.C.on(e.getWrapperElement(),"mouseout",n.onMouseOut),i.C.on(document,"keydown",n.onKeyDown)}})),o(u,"onMouseOver"),o(l,"onMouseOut"),o(s,"onKeyDown");const p="undefined"!=typeof navigator&&navigator&&-1!==navigator.appVersion.indexOf("Mac");function c(e){return e===(p?"Meta":"Control")}function f(e){if(e.state.jump.marker)return;const t=e.state.jump.cursor,n=e.coordsChar(t),i=e.getTokenAt(n,!0),a=e.state.jump.options,r=a.getDestination||e.getHelper(n,"jump");if(r){const t=r(i,a,e);if(t){const a=e.markText({line:n.line,ch:i.start},{line:n.line,ch:i.end},{className:"CodeMirror-jump-token"});e.state.jump.marker=a,e.state.jump.destination=t}}}function d(e){const t=e.state.jump.marker;e.state.jump.marker=null,e.state.jump.destination=null,t.clear()}o(c,"isJumpModifier"),o(f,"enableJumpMode"),o(d,"disableJumpMode"),i.C.registerHelper("jump","graphql",((e,t)=>{if(!t.schema||!t.onClick||!e.state)return;const n=e.state,i=n.kind,r=n.step,o=(0,a.g)(t.schema,n);return"Field"===i&&0===r&&o.fieldDef||"AliasedField"===i&&2===r&&o.fieldDef?(0,a.a)(o):"Directive"===i&&1===r&&o.directiveDef?(0,a.b)(o):"Argument"===i&&0===r&&o.argDef?(0,a.c)(o):"EnumValue"===i&&o.enumValue?(0,a.d)(o):"NamedType"===i&&o.type?(0,a.e)(o):void 0}))}}]);