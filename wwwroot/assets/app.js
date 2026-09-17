const API = "/api";

const enums = {
  yakitTuru: ["Benzin","Dizel","Elektrik","Hibrit"],
  aracDurumu: ["Müsait","Görevde","Serviste","Arızalı","Rezerve"],
  aracTuru: ["Otomobil","Kamyonet","Minibüs","Kamyon"],
  ehliyet: ["B","C","D"],
  gorevDurumu: ["Planlandı","Devam Ediyor","Tamamlandı","İptal Edildi"]
};

const state = {
  page: "dashboard",
  araclar: [],
  calisanlar: [],
  gorevler: [],
  yakit: [],
  bakim: [],
  hasar: []
};

const titles = {
  dashboard:["Dashboard","Filo durumunu tek ekrandan yönetin."],
  araclar:["Araçlar","Filo araçlarını ekleyin, güncelleyin ve takip edin."],
  calisanlar:["Çalışanlar","Sürücü ve personel kayıtlarını yönetin."],
  gorevler:["Görevler","Görev planlayın, başlatın ve tamamlayın."],
  yakit:["Yakıt Kayıtları","Araç yakıt tüketimi ve maliyetlerini izleyin."],
  bakim:["Bakım Kayıtları","Bakım geçmişini ve sonraki bakım kilometrelerini yönetin."],
  hasar:["Hasar Kayıtları","Hasar, görev ve maliyet bilgilerini takip edin."],
  raporlar:["Raporlar","Filo maliyetleri ve operasyonel özetleri görüntüleyin."]
};

const $ = (s, root=document)=>root.querySelector(s);
const $$ = (s, root=document)=>[...root.querySelectorAll(s)];
const fmtDate = v => v ? new Date(v).toLocaleString("tr-TR") : "-";
const fmtMoney = v => new Intl.NumberFormat("tr-TR",{style:"currency",currency:"TRY"}).format(Number(v||0));
const safe = v => (v ?? "-");

function toast(msg, type="success"){
  const el=document.createElement("div");
  el.className=`toast ${type}`;
  el.textContent=msg;
  $("#toast-container").appendChild(el);
  setTimeout(()=>el.remove(),3500);
}

async function api(path, options={}){
  const opts = {headers:{"Content-Type":"application/json"}, ...options};
  if(options.body && typeof options.body !== "string") opts.body = JSON.stringify(options.body);
  const res = await fetch(`${API}${path}`, opts);
  if(res.status===204) return null;
  const text = await res.text();
  let data = null;
  try{ data = text ? JSON.parse(text) : null; } catch { data = text; }
  if(!res.ok){
    let msg="İşlem başarısız.";
    if(typeof data==="string") msg=data;
    else if(data?.title) msg=data.title;
    else if(data?.errors) msg=Object.values(data.errors).flat().join(" ");
    throw new Error(msg);
  }
  return data;
}

function badge(text, cls="gray"){ return `<span class="badge ${cls}">${text}</span>`; }
function statusBadge(d){
  const map=[["Müsait","success"],["Görevde","warning"],["Serviste","gray"],["Arızalı","danger"],["Rezerve","gray"]];
  return badge(map[d]?.[0] ?? d, map[d]?.[1] ?? "gray");
}
function gorevBadge(d){
  const map=[["Planlandı","gray"],["Devam Ediyor","warning"],["Tamamlandı","success"],["İptal Edildi","danger"]];
  return badge(map[d]?.[0] ?? d, map[d]?.[1] ?? "gray");
}

function openModal(title, html, onSubmit){
  $("#modal-title").textContent=title;
  $("#modal-body").innerHTML=html;
  $("#modal").classList.remove("hidden");
  const form=$("#modal-body form");
  if(form && onSubmit){
    form.addEventListener("submit", async e=>{
      e.preventDefault();
      const fd=new FormData(form);
      const obj=Object.fromEntries(fd.entries());
      try{
        await onSubmit(obj);
        closeModal();
      }catch(err){ toast(err.message,"error"); }
    });
  }
}
function closeModal(){ $("#modal").classList.add("hidden"); }
document.addEventListener("click", e=>{ if(e.target.matches("[data-close-modal]")) closeModal(); });

function enumOptions(items, value){
  return items.map((x,i)=>`<option value="${i}" ${String(i)===String(value)?"selected":""}>${x}</option>`).join("");
}
function selectOptions(items, value, labeler){
  return items.map(x=>`<option value="${x.id}" ${String(x.id)===String(value)?"selected":""}>${labeler(x)}</option>`).join("");
}

async function loadBase(){
  const [araclar,calisanlar,gorevler] = await Promise.all([
    api("/Arac"), api("/Calisan"), api("/Gorev")
  ]);
  state.araclar=araclar; state.calisanlar=calisanlar; state.gorevler=gorevler;
}

async function renderDashboard(){
  await loadBase();
  const musait = state.araclar.filter(x=>x.durum===0).length;
  const gorevde = state.araclar.filter(x=>x.durum===1).length;
  const devam = state.gorevler.filter(x=>x.durum===1).length;
  $("#content").innerHTML=`
    <div class="cards">
      <div class="card"><div class="metric-label">Toplam Araç</div><div class="metric-value">${state.araclar.length}</div></div>
      <div class="card"><div class="metric-label">Müsait Araç</div><div class="metric-value">${musait}</div></div>
      <div class="card"><div class="metric-label">Görevde Araç</div><div class="metric-value">${gorevde}</div></div>
      <div class="card"><div class="metric-label">Çalışan</div><div class="metric-value">${state.calisanlar.length}</div></div>
    </div>
    <div class="grid-2">
      <div class="card">
        <div class="section-head"><h2>Devam Eden Görevler</h2><span>${devam} aktif</span></div>
        ${state.gorevler.filter(x=>x.durum===1).slice(0,6).map(x=>`
          <div class="kv"><span>${x.amac} · ${x.aracPlaka}</span><strong>${x.calisanAdSoyad}</strong></div>
        `).join("") || '<div class="empty">Devam eden görev yok.</div>'}
      </div>
      <div class="card">
        <div class="section-head"><h2>En Yüksek Kilometre</h2></div>
        ${[...state.araclar].sort((a,b)=>b.kilometre-a.kilometre).slice(0,6).map(x=>`
          <div class="kv"><span>${x.plaka} · ${x.marka} ${x.model}</span><strong>${x.kilometre.toLocaleString("tr-TR")} km</strong></div>
        `).join("") || '<div class="empty">Araç kaydı yok.</div>'}
      </div>
    </div>`;
}

function tableShell(title, buttonText, rows, headers){
  return `<div class="card">
    <div class="section-head"><h2>${title}</h2>${buttonText?`<button class="btn" id="add-btn">${buttonText}</button>`:""}</div>
    <div class="table-wrap"><table><thead><tr>${headers.map(h=>`<th>${h}</th>`).join("")}</tr></thead><tbody>${rows}</tbody></table></div>
  </div>`;
}

async function renderAraclar(){
  state.araclar=await api("/Arac");
  const rows=state.araclar.map(a=>`<tr>
    <td>${a.id}</td><td><strong>${a.plaka}</strong></td><td>${a.marka} ${a.model}</td>
    <td>${a.modelYili}</td><td>${a.kilometre.toLocaleString("tr-TR")} km</td>
    <td>${enums.yakitTuru[a.yakitTuru]}</td><td>${enums.aracTuru[a.aracTuru]}</td>
    <td>${enums.ehliyet[a.gerekliEhliyetSinifi]}</td><td>${statusBadge(a.durum)}</td>
    <td class="actions"><button class="btn small ghost" data-edit-arac="${a.id}">Düzenle</button><button class="btn small danger" data-del-arac="${a.id}">Sil</button></td>
  </tr>`).join("");
  $("#content").innerHTML=tableShell("Araç Listesi","Yeni Araç",rows,["ID","Plaka","Araç","Yıl","KM","Yakıt","Tür","Ehliyet","Durum","İşlem"]);
  $("#add-btn").onclick=()=>aracForm();
  $$("[data-edit-arac]").forEach(b=>b.onclick=()=>aracForm(Number(b.dataset.editArac)));
  $$("[data-del-arac]").forEach(b=>b.onclick=()=>deleteConfirm("Araç", async()=>api(`/Arac/${b.dataset.delArac}`,{method:"DELETE"}),renderAraclar));
}

function aracForm(id){
  const a=state.araclar.find(x=>x.id===id)||{};
  openModal(id?"Araç Düzenle":"Yeni Araç",`
  <form><div class="form-grid">
    <div class="field"><label>Plaka</label><input name="plaka" required minlength="5" maxlength="15" value="${a.plaka||""}"></div>
    <div class="field"><label>Marka</label><input name="marka" required value="${a.marka||""}"></div>
    <div class="field"><label>Model</label><input name="model" required value="${a.model||""}"></div>
    <div class="field"><label>Model Yılı</label><input type="number" name="modelYili" min="1990" max="2100" required value="${a.modelYili||new Date().getFullYear()}"></div>
    <div class="field"><label>Kilometre</label><input type="number" name="kilometre" min="0" required value="${a.kilometre||0}"></div>
    <div class="field"><label>Yakıt Türü</label><select name="yakitTuru">${enumOptions(enums.yakitTuru,a.yakitTuru)}</select></div>
    <div class="field"><label>Araç Türü</label><select name="aracTuru">${enumOptions(enums.aracTuru,a.aracTuru)}</select></div>
    <div class="field"><label>Gerekli Ehliyet</label><select name="gerekliEhliyetSinifi">${enumOptions(enums.ehliyet,a.gerekliEhliyetSinifi)}</select></div>
  </div><div class="form-actions"><button type="button" class="btn ghost" data-close-modal>İptal</button><button class="btn">Kaydet</button></div></form>`,
  async o=>{
    ["modelYili","kilometre","yakitTuru","aracTuru","gerekliEhliyetSinifi"].forEach(k=>o[k]=Number(o[k]));
    await api(id?`/Arac/${id}`:"/Arac",{method:id?"PUT":"POST",body:o});
    toast("Araç kaydedildi."); await renderAraclar();
  });
}

async function renderCalisanlar(){
  state.calisanlar=await api("/Calisan");
  const rows=state.calisanlar.map(c=>`<tr><td>${c.id}</td><td><strong>${c.adSoyad}</strong></td><td>${c.pozisyon}</td><td>${c.telefonNo}</td><td>${c.ehliyet==null?"Yok":enums.ehliyet[c.ehliyet]}</td>
  <td class="actions"><button class="btn small ghost" data-edit-cal="${c.id}">Düzenle</button><button class="btn small danger" data-del-cal="${c.id}">Sil</button></td></tr>`).join("");
  $("#content").innerHTML=tableShell("Çalışan Listesi","Yeni Çalışan",rows,["ID","Ad Soyad","Pozisyon","Telefon","Ehliyet","İşlem"]);
  $("#add-btn").onclick=()=>calisanForm();
  $$("[data-edit-cal]").forEach(b=>b.onclick=()=>calisanForm(Number(b.dataset.editCal)));
  $$("[data-del-cal]").forEach(b=>b.onclick=()=>deleteConfirm("Çalışan", async()=>api(`/Calisan/${b.dataset.delCal}`,{method:"DELETE"}),renderCalisanlar));
}
function calisanForm(id){
  const c=state.calisanlar.find(x=>x.id===id)||{};
  openModal(id?"Çalışan Düzenle":"Yeni Çalışan",`
  <form><div class="form-grid">
    <div class="field full"><label>Ad Soyad</label><input name="adSoyad" required value="${c.adSoyad||""}"></div>
    <div class="field"><label>Pozisyon</label><input name="pozisyon" required value="${c.pozisyon||""}"></div>
    <div class="field"><label>Telefon No</label><input name="telefonNo" required value="${c.telefonNo||""}"></div>
    <div class="field"><label>Ehliyet</label><select name="ehliyet"><option value="">Ehliyet yok</option>${enumOptions(enums.ehliyet,c.ehliyet)}</select></div>
  </div><div class="form-actions"><button type="button" class="btn ghost" data-close-modal>İptal</button><button class="btn">Kaydet</button></div></form>`,
  async o=>{
    o.ehliyet=o.ehliyet===""?null:Number(o.ehliyet);
    await api(id?`/Calisan/${id}`:"/Calisan",{method:id?"PUT":"POST",body:o});
    toast("Çalışan kaydedildi."); await renderCalisanlar();
  });
}

async function renderGorevler(){
  await loadBase();
  const rows=state.gorevler.map(g=>`<tr>
    <td>${g.id}</td><td><strong>${g.amac}</strong><br><span style="color:#6b7280">${g.kalkisYeri} → ${g.varisYeri}</span></td>
    <td>${g.aracPlaka}</td><td>${g.calisanAdSoyad}</td><td>${fmtDate(g.planlananCikis)}</td><td>${fmtDate(g.planlananDonus)}</td>
    <td>${gorevBadge(g.durum)}</td><td>${safe(g.baslangicKm)} / ${safe(g.bitisKm)}</td>
    <td class="actions">
      ${g.durum===0?`<button class="btn small success" data-start="${g.id}">Başlat</button>`:""}
      ${g.durum===1?`<button class="btn small warning" data-finish="${g.id}">Tamamla</button>`:""}
      <button class="btn small ghost" data-edit-gorev="${g.id}">Düzenle</button>
      <button class="btn small danger" data-del-gorev="${g.id}">Sil</button>
    </td></tr>`).join("");
  $("#content").innerHTML=tableShell("Görev Listesi","Yeni Görev",rows,["ID","Görev","Araç","Çalışan","Planlanan Çıkış","Planlanan Dönüş","Durum","KM","İşlem"]);
  $("#add-btn").onclick=()=>gorevForm();
  $$("[data-edit-gorev]").forEach(b=>b.onclick=()=>gorevForm(Number(b.dataset.editGorev)));
  $$("[data-del-gorev]").forEach(b=>b.onclick=()=>deleteConfirm("Görev",async()=>api(`/Gorev/${b.dataset.delGorev}`,{method:"DELETE"}),renderGorevler));
  $$("[data-start]").forEach(b=>b.onclick=async()=>{try{await api(`/Gorev/${b.dataset.start}/baslat`,{method:"PUT"});toast("Görev başlatıldı.");renderGorevler();}catch(e){toast(e.message,"error")}});
  $$("[data-finish]").forEach(b=>b.onclick=()=>finishForm(Number(b.dataset.finish)));
}
function gorevForm(id){
  const g=state.gorevler.find(x=>x.id===id)||{};
  const dt=v=>v?new Date(v).toISOString().slice(0,16):"";
  openModal(id?"Görev Düzenle":"Yeni Görev",`
  <form><div class="form-grid">
    <div class="field full"><label>Amaç</label><input name="amac" required value="${g.amac||""}"></div>
    <div class="field"><label>Kalkış Yeri</label><input name="kalkisYeri" required value="${g.kalkisYeri||""}"></div>
    <div class="field"><label>Varış Yeri</label><input name="varisYeri" required value="${g.varisYeri||""}"></div>
    <div class="field"><label>Araç</label><select name="aracId">${selectOptions(state.araclar,g.aracId,a=>`${a.plaka} · ${enums.ehliyet[a.gerekliEhliyetSinifi]}`)}</select></div>
    <div class="field"><label>Çalışan</label><select name="calisanId">${selectOptions(state.calisanlar,g.calisanId,c=>`${c.adSoyad} · ${c.ehliyet==null?"Ehliyet yok":enums.ehliyet[c.ehliyet]}`)}</select></div>
    <div class="field"><label>Planlanan Çıkış</label><input type="datetime-local" name="planlananCikis" required value="${dt(g.planlananCikis)}"></div>
    <div class="field"><label>Planlanan Dönüş</label><input type="datetime-local" name="planlananDonus" required value="${dt(g.planlananDonus)}"></div>
  </div><div class="form-actions"><button type="button" class="btn ghost" data-close-modal>İptal</button><button class="btn">Kaydet</button></div></form>`,
  async o=>{
    o.aracId=Number(o.aracId); o.calisanId=Number(o.calisanId);
    await api(id?`/Gorev/${id}`:"/Gorev",{method:id?"PUT":"POST",body:o});
    toast("Görev kaydedildi."); await renderGorevler();
  });
}
function finishForm(id){
  openModal("Görevi Tamamla",`<form><div class="field"><label>Bitiş Kilometresi</label><input type="number" name="bitisKm" min="0" required></div><div class="form-actions"><button type="button" class="btn ghost" data-close-modal>İptal</button><button class="btn">Tamamla</button></div></form>`,
  async o=>{o.bitisKm=Number(o.bitisKm);await api(`/Gorev/${id}/tamamla`,{method:"PUT",body:o});toast("Görev tamamlandı.");await renderGorevler();});
}

async function renderYakit(){
  state.yakit=await api("/Yakit"); state.araclar=await api("/Arac");
  const rows=state.yakit.map(x=>`<tr><td>${x.id}</td><td>${x.aracPlaka}</td><td>${fmtDate(x.tarih)}</td><td>${x.litre} L</td><td>${fmtMoney(x.toplamUcret)}</td><td>${x.kilometre} km</td><td>${enums.yakitTuru[x.yakitTuru]}</td>
  <td class="actions"><button class="btn small ghost" data-edit-yakit="${x.id}">Düzenle</button><button class="btn small danger" data-del-yakit="${x.id}">Sil</button></td></tr>`).join("");
  $("#content").innerHTML=tableShell("Yakıt Kayıtları","Yeni Yakıt Kaydı",rows,["ID","Araç","Tarih","Litre","Tutar","KM","Yakıt","İşlem"]);
  $("#add-btn").onclick=()=>yakitForm();
  $$("[data-edit-yakit]").forEach(b=>b.onclick=()=>yakitForm(Number(b.dataset.editYakit)));
  $$("[data-del-yakit]").forEach(b=>b.onclick=()=>deleteConfirm("Yakıt kaydı",async()=>api(`/Yakit/${b.dataset.delYakit}`,{method:"DELETE"}),renderYakit));
}
function yakitForm(id){
  const x=state.yakit.find(v=>v.id===id)||{}; const dt=v=>v?new Date(v).toISOString().slice(0,16):"";
  openModal(id?"Yakıt Kaydı Düzenle":"Yeni Yakıt Kaydı",`<form><div class="form-grid">
    <div class="field"><label>Araç</label><select name="aracId">${selectOptions(state.araclar,x.aracId,a=>a.plaka)}</select></div>
    <div class="field"><label>Tarih</label><input type="datetime-local" name="tarih" required value="${dt(x.tarih)}"></div>
    <div class="field"><label>Litre</label><input type="number" step="0.01" min="0.01" name="litre" required value="${x.litre??""}"></div>
    <div class="field"><label>Toplam Ücret</label><input type="number" step="0.01" min="0" name="toplamUcret" required value="${x.toplamUcret??""}"></div>
    <div class="field"><label>Kilometre</label><input type="number" min="0" name="kilometre" required value="${x.kilometre??""}"></div>
    <div class="field"><label>Yakıt Türü</label><select name="yakitTuru">${enumOptions(enums.yakitTuru,x.yakitTuru)}</select></div>
  </div><div class="form-actions"><button type="button" class="btn ghost" data-close-modal>İptal</button><button class="btn">Kaydet</button></div></form>`,async o=>{
    ["aracId","litre","toplamUcret","kilometre","yakitTuru"].forEach(k=>o[k]=Number(o[k]));
    await api(id?`/Yakit/${id}`:"/Yakit",{method:id?"PUT":"POST",body:o});toast("Yakıt kaydı kaydedildi.");await renderYakit();
  });
}

async function renderBakim(){
  state.bakim=await api("/Bakim"); state.araclar=await api("/Arac");
  const rows=state.bakim.map(x=>`<tr><td>${x.id}</td><td>${x.aracPlaka}</td><td>${x.bakimTuru}</td><td>${fmtDate(x.tarih)}</td><td>${fmtMoney(x.toplamUcret)}</td><td>${x.kilometre} km</td><td>${safe(x.sonrakiBakimKm)}</td><td>${safe(x.aciklama)}</td>
  <td class="actions"><button class="btn small ghost" data-edit-bakim="${x.id}">Düzenle</button><button class="btn small danger" data-del-bakim="${x.id}">Sil</button></td></tr>`).join("");
  $("#content").innerHTML=tableShell("Bakım Kayıtları","Yeni Bakım Kaydı",rows,["ID","Araç","Bakım","Tarih","Tutar","KM","Sonraki Bakım KM","Açıklama","İşlem"]);
  $("#add-btn").onclick=()=>bakimForm();
  $$("[data-edit-bakim]").forEach(b=>b.onclick=()=>bakimForm(Number(b.dataset.editBakim)));
  $$("[data-del-bakim]").forEach(b=>b.onclick=()=>deleteConfirm("Bakım kaydı",async()=>api(`/Bakim/${b.dataset.delBakim}`,{method:"DELETE"}),renderBakim));
}
function bakimForm(id){
  const x=state.bakim.find(v=>v.id===id)||{}; const dt=v=>v?new Date(v).toISOString().slice(0,16):"";
  openModal(id?"Bakım Kaydı Düzenle":"Yeni Bakım Kaydı",`<form><div class="form-grid">
    <div class="field"><label>Araç</label><select name="aracId">${selectOptions(state.araclar,x.aracId,a=>a.plaka)}</select></div>
    <div class="field"><label>Tarih</label><input type="datetime-local" name="tarih" required value="${dt(x.tarih)}"></div>
    <div class="field"><label>Bakım Türü</label><input name="bakimTuru" required value="${x.bakimTuru||""}"></div>
    <div class="field"><label>Toplam Ücret</label><input type="number" step="0.01" min="0" name="toplamUcret" required value="${x.toplamUcret??""}"></div>
    <div class="field"><label>Kilometre</label><input type="number" min="0" name="kilometre" required value="${x.kilometre??""}"></div>
    <div class="field"><label>Sonraki Bakım KM</label><input type="number" min="0" name="sonrakiBakimKm" value="${x.sonrakiBakimKm??""}"></div>
    <div class="field full"><label>Açıklama</label><textarea name="aciklama">${x.aciklama||""}</textarea></div>
  </div><div class="form-actions"><button type="button" class="btn ghost" data-close-modal>İptal</button><button class="btn">Kaydet</button></div></form>`,async o=>{
    o.aracId=Number(o.aracId);o.toplamUcret=Number(o.toplamUcret);o.kilometre=Number(o.kilometre);o.sonrakiBakimKm=o.sonrakiBakimKm===""?null:Number(o.sonrakiBakimKm);
    await api(id?`/Bakim/${id}`:"/Bakim",{method:id?"PUT":"POST",body:o});toast("Bakım kaydı kaydedildi.");await renderBakim();
  });
}

async function renderHasar(){
  state.hasar=await api("/Hasar"); await loadBase();
  const rows=state.hasar.map(x=>`<tr><td>${x.id}</td><td>${x.aracPlaka}</td><td>${safe(x.gorevAmaci)}</td><td>${x.durum}</td><td>${fmtDate(x.tarih)}</td><td>${fmtMoney(x.maliyet)}</td><td>${x.aciklama}</td>
  <td class="actions"><button class="btn small ghost" data-edit-hasar="${x.id}">Düzenle</button><button class="btn small danger" data-del-hasar="${x.id}">Sil</button></td></tr>`).join("");
  $("#content").innerHTML=tableShell("Hasar Kayıtları","Yeni Hasar Kaydı",rows,["ID","Araç","Görev","Durum","Tarih","Maliyet","Açıklama","İşlem"]);
  $("#add-btn").onclick=()=>hasarForm();
  $$("[data-edit-hasar]").forEach(b=>b.onclick=()=>hasarForm(Number(b.dataset.editHasar)));
  $$("[data-del-hasar]").forEach(b=>b.onclick=()=>deleteConfirm("Hasar kaydı",async()=>api(`/Hasar/${b.dataset.delHasar}`,{method:"DELETE"}),renderHasar));
}
function hasarForm(id){
  const x=state.hasar.find(v=>v.id===id)||{}; const dt=v=>v?new Date(v).toISOString().slice(0,16):"";
  openModal(id?"Hasar Kaydı Düzenle":"Yeni Hasar Kaydı",`<form><div class="form-grid">
    <div class="field"><label>Araç</label><select name="aracId">${selectOptions(state.araclar,x.aracId,a=>a.plaka)}</select></div>
    <div class="field"><label>Görev (opsiyonel)</label><select name="gorevId"><option value="">Görev yok</option>${selectOptions(state.gorevler,x.gorevId,g=>`${g.id} · ${g.amac} · ${g.aracPlaka}`)}</select></div>
    <div class="field"><label>Durum</label><input name="durum" required value="${x.durum||""}"></div>
    <div class="field"><label>Tarih</label><input type="datetime-local" name="tarih" required value="${dt(x.tarih)}"></div>
    <div class="field"><label>Maliyet</label><input type="number" step="0.01" min="0.01" name="maliyet" value="${x.maliyet??""}"></div>
    <div class="field full"><label>Açıklama</label><textarea name="aciklama" required>${x.aciklama||""}</textarea></div>
  </div><div class="form-actions"><button type="button" class="btn ghost" data-close-modal>İptal</button><button class="btn">Kaydet</button></div></form>`,async o=>{
    o.aracId=Number(o.aracId);o.gorevId=o.gorevId===""?null:Number(o.gorevId);o.maliyet=o.maliyet===""?null:Number(o.maliyet);
    await api(id?`/Hasar/${id}`:"/Hasar",{method:id?"PUT":"POST",body:o});toast("Hasar kaydı kaydedildi.");await renderHasar();
  });
}

async function renderRaporlar(){
  state.araclar=await api("/Arac");
  const gorevSayilari=await api("/Rapor/calisan-gorev-sayilari");
  const km=await api("/Rapor/araclar-kilometre");
  $("#content").innerHTML=`
    <div class="grid-2">
      <div class="card">
        <div class="section-head"><h2>Araç Maliyet Raporu</h2></div>
        <div class="report-controls"><select id="report-arac">${selectOptions(state.araclar,null,a=>`${a.plaka} · ${a.marka} ${a.model}`)}</select><button id="report-btn" class="btn">Getir</button></div>
        <div id="report-result" class="report-result">Bir araç seçip raporu getir.</div>
      </div>
      <div class="card">
        <div class="section-head"><h2>Çalışan Görev Sayıları</h2></div>
        ${gorevSayilari.map(x=>`<div class="kv"><span>${x.calisanAdSoyad}</span><strong>${x.gorevSayisi}</strong></div>`).join("") || '<div class="empty">Veri yok.</div>'}
      </div>
      <div class="card">
        <div class="section-head"><h2>Kilometre Sıralaması</h2></div>
        ${km.slice(0,10).map(x=>`<div class="kv"><span>${x.plaka} · ${x.marka} ${x.model}</span><strong>${x.kilometre.toLocaleString("tr-TR")} km</strong></div>`).join("")}
      </div>
      <div class="card">
        <div class="section-head"><h2>Müsait Araçlar</h2></div>
        <div id="musait-list">Yükleniyor...</div>
      </div>
    </div>`;
  const musait=await api("/Rapor/musait-araclar");
  $("#musait-list").innerHTML=musait.map(x=>`<div class="kv"><span>${x.plaka}</span><strong>${x.marka} ${x.model}</strong></div>`).join("")||'<div class="empty">Müsait araç yok.</div>';
  $("#report-btn").onclick=async()=>{
    try{
      const id=$("#report-arac").value;
      const r=await api(`/Rapor/arac-toplam-maliyet/${id}`);
      $("#report-result").innerHTML=`
        <div class="kv"><span>Plaka</span><strong>${r.plaka}</strong></div>
        <div class="kv"><span>Yakıt</span><strong>${fmtMoney(r.yakitMaliyeti)}</strong></div>
        <div class="kv"><span>Bakım</span><strong>${fmtMoney(r.bakimMaliyeti)}</strong></div>
        <div class="kv"><span>Hasar</span><strong>${fmtMoney(r.hasarMaliyeti)}</strong></div>
        <div class="kv"><span>Toplam</span><strong>${fmtMoney(r.toplamMaliyet)}</strong></div>`;
    }catch(e){toast(e.message,"error")}
  };
}

function deleteConfirm(name, fn, refresh){
  openModal(`${name} Sil`, `<p>Bu kaydı silmek istediğinize emin misiniz?</p><div class="form-actions"><button class="btn ghost" data-close-modal>Vazgeç</button><button id="confirm-delete" class="btn danger">Sil</button></div>`);
  $("#confirm-delete").onclick=async()=>{try{await fn();closeModal();toast("Kayıt silindi.");await refresh();}catch(e){toast(e.message,"error")}};
}

async function render(){
  const [title,sub]=titles[state.page];
  $("#page-title").textContent=title; $("#page-subtitle").textContent=sub;
  $("#content").innerHTML='<div class="card"><div class="empty">Yükleniyor...</div></div>';
  try{
    const map={dashboard:renderDashboard,araclar:renderAraclar,calisanlar:renderCalisanlar,gorevler:renderGorevler,yakit:renderYakit,bakim:renderBakim,hasar:renderHasar,raporlar:renderRaporlar};
    await map[state.page]();
    $("#api-status").className="status-dot online";
  }catch(e){
    $("#api-status").className="status-dot offline";
    $("#content").innerHTML=`<div class="card"><div class="empty"><strong>API bağlantısı kurulamadı.</strong><br><br>${e.message}</div></div>`;
    toast(e.message,"error");
  }
}

$$(".nav-item").forEach(b=>b.onclick=()=>{
  $$(".nav-item").forEach(x=>x.classList.remove("active")); b.classList.add("active");
  state.page=b.dataset.page; render();
});
$("#refresh-btn").onclick=render;
render();
