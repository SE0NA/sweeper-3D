# sweeper-3D

<img src="" width="300" height="230">

<h5>
sweeper3D는 기존의 고전 게임인 지뢰 찾기를 1인칭 3D의 게임으로 재설계한 게임이다. <br>
플레이어는 스테이지를 돌아다니며 지뢰가 있는 방을 제외한 모든 방을 탐색하는 것을 목표로 게임을 진행한다. <br><br>
플레이어는 반드시 어느 하나의 방에 위치하게 되며, 그 위치한 방을 중심으로 십자 형태의 주변 4개의 방에 대하여
그 중 지뢰가 있는 방은 몇 개인지에 대한 정보를 얻을 수 있다. 해당 정보를 이용하여 플레이어는 여러 방을 돌아다니며 지뢰가 있는 방과 없는 방을 추리한다.<br><br>
마우스 클릭을 이용하여 지뢰가 있다고 생각하는 방에 깃발 표시를 하거나, 지뢰가 없다고 생각하는 방을 열어 다른 방으로의 이동이 가능하다.<br>
이를 반복하여 지뢰를 제외한 모든 방을 탐색하면 게임이 클리어 된다.<br><br>
기존의 지뢰 찾기에서 추가된 요소로는 몬스터와 멀티플레이 기능이 있다.<br>
플레이어는 게임 중반에 등장하는 몬스터를 피하여 게임을 진행한다.<br>
멀티 플레이 기능을 이용하여 여러 명의 플레이어가 함께 협동하여 게임을 진행할 수 있다.
</h5>

###### sweeper3D는 [3D 지뢰 찾기](https://github.com/SE0NA/3DGameProject)를 재설계·보완·발전하여 구현되었다.<br> → 몬스터와 멀티플레이 요소 추가 / 디자인 변경


***
##### 1. 프로젝트 명: sweeper3D
##### 2. 개인 프로젝트
***

### 게임 모습

##### 1. 게임 시작 화면
<div>
<img src="https://user-images.githubusercontent.com/85846475/205488170-f22cb571-7c24-4aef-8b2c-27ca31376fdc.png" width="150" height="150"><br>
<img src="https://user-images.githubusercontent.com/85846475/205488269-4d5fede1-6e9d-45ed-9eed-e772ceb332a5.png" width="350" height="230">
<img src="https://user-images.githubusercontent.com/85846475/205488351-351e982e-9870-4f26-a96b-fa7db72d9d85.png" width="350" height="230"><br>
<h6>게임 실행 아이콘과 게임을 실행했을 때의 화면이다.<br>
시작 화면에는 게임 시작 버튼과 소개 버튼, 종료 버튼이 있다. 소개 버튼을 클릭하면, 오른쪽 그림과 같이 게임을 소개하는 UI를 확인할 수 있다.</h6>
</div>
<br><br>

##### 2. 게임 로비
<div>
<img src="https://user-images.githubusercontent.com/85846475/205488461-4f835cfc-6b13-4c53-a0af-9cccb138be34.png" width="350" height="230">
<img src="https://user-images.githubusercontent.com/85846475/205488485-ac830ed7-a3e0-421f-a118-36ff56d6a8fb.png">
<h6>게임을 플레이하기 위해서는 네트워크에 접속해야 한다. 네트워크는 Photon PUN2를 이용하였다.<br>
닉네임은 필수이며, 방에 따라 방코드를 입력한다. 올바른 입력과 온라인 상태라면, 방에 입장한다.
</div>
<br><br>

##### 3. 방 로비
<div><h6>
<img src="https://user-images.githubusercontent.com/85846475/205488598-83d48bab-dc74-47cd-8907-42c282541617.png" width="350" height="230"><br>
원하는 방에 입장하면 같은 방의 플레이어들과 채팅, 설정된 난이도, 방 코드를 확인할 수 있다.<br>
해당 화면은 일반 참가자의 화면이다.</h6>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205488698-00d9e06e-b282-4313-b23d-7f91f40a2488.png">
<img src="https://user-images.githubusercontent.com/85846475/205488717-667f74bf-e43e-4dfe-9576-fd344516e182.png"><br>
랜덤 방에 입장하면 방 코드 부분에 랜덤 방임을 표시하며,<br>해당 방에 플레이어가 본인 혼자인 경우 싱글 플레이로 게임을 진행할 수 있음을 알린다.</h6>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205488826-ad355f7c-c2e0-4d7e-934f-21650a4bc441.png" width="350" height="230">
<img src="https://user-images.githubusercontent.com/85846475/205488851-515e7f3d-b9c5-46a8-b357-4635d93fef4f.png" width="350" height="230"><br>
마스터 클라이언트의 화면이다. 마스터 클라이언트는 게임의 난이도를 설정할 수 있는 권한을 갖는다.<br>
난이도 설정 창에서 이를 변경할 수 있다.</h6>
</div>
<br>

##### 4. 게임 스테이지
<div><h6>
<img src="https://user-images.githubusercontent.com/85846475/205489044-3aba9ac2-60ce-4814-a73b-3eae4ba75b4e.png" width="350" height="230"><br>
게임은 키보드의 W, A, S, D 키를 이용한 이동, 마우스 클릭을 이용한 문과의 상호작용을 통해 진행된다.<br>
플레이어는 각 방에 들어갈 때마다 해당 방의 주변의 지뢰가 있는 방의 수를 UI에서 확인할 수 있다.<br></h6>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205489248-a24d8e87-c4a8-42cb-82ab-e3dce6379caf.png" width="300" height="200">
<img src="https://user-images.githubusercontent.com/85846475/205489275-83869591-3659-4f82-b89d-3cd37b17dada.png" width="300" height="200">
<img src="https://user-images.githubusercontent.com/85846475/205489296-0ee9e819-380c-4e18-ab4d-bb44acb9a978.png" width="300" height="200"><br>
문과의 상호작용을 통해 위와 같은 결과를 얻을 수 있다.<br>
플레이어 오브젝트는 문의 Collider 범위에 들어서면, 문과의 상호작용이 가능함을 알리는 안내를 확인할 수 있다.<br>
마우스 왼쪽 클릭은 해당 문을 열어 방을 오픈하며, 오른쪽 클릭은 지뢰 표시로 해당 방에 지뢰가 있음을 알리기 위해 방을 둘러싼 문의 조명을 노랗게 한다.<br>
해당 동작이 게임을 진행하는 메인이다.</h6>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205489496-e06fcf0e-7198-44ab-954f-787a2d56d4c4.png" width="350" height="230"><br>
텔레포트 기능을 이용하여 먼 위치의 방을 빠르게 이동하거나, 접근할 수 없는 방으로 이동할 수 있다.
</h6>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205489567-40ebacc0-e263-4311-a0cd-9bc6139e9d38.png"><br>
주변 지뢰 수를 알려주는 UI이다. 어떤 방에 입장할 때마다 변경된다.
</h6><br>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205489623-416d3a90-040d-41eb-bb43-26e1bf6b07b4.png" width="350" height="230">
<img src="https://user-images.githubusercontent.com/85846475/205489647-bc0610b1-2348-421b-bc1d-9df436c3b217.png" width="350" height="230"><br>
모든 방을 열어 게임이 클리어되거나 지뢰가 있는 방을 열어 게임이 오버될 수 있다.
</h6>
</div>
<br><br>

##### 5. 몬스터
<div><h6>
<img src="https://user-images.githubusercontent.com/85846475/205489768-44744ff3-4ba4-4ce8-9856-dece7fbde14d.png" width="350" height="230">
<img src="https://user-images.githubusercontent.com/85846475/205489800-aa4e354c-a5ec-44f9-97b8-b4cacd8929cb.png" width="350" height="230"><br>
몬스터가 생성되면 이를 경고음과 함께 안내한다.<br>
플레이어가 몬스터의 탐색 범위(Collider)에 위치하면 텔레포트 기능은 사용할 수 없다.<br>
</h6>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205490040-0675763c-cfdf-4bee-9895-0e74f44348be.png" width="150" height="150">
<img src="https://user-images.githubusercontent.com/85846475/205490051-c123f484-1217-4e6c-85f1-8fa0ab162df2.png" width="150" height="150"><br>
몬스터는 유니티의 AINavigation기능을 적용하여 스테이지를 랜덤으로 순찰하며, Raycast를 통해 플레이어를 탐색한다.<br>
플레이어가 발견되면 플레이어의 위치를 지속적으로 목적지로 설정하여 타겟 플레이어를 추적한다.<br>
플레이어가 탐색 범위(Collider)를 벗어나면, 도망친 것으로 판단하여 다시 순찰을 진행한다.</h6>
<h6>
<img src="https://user-images.githubusercontent.com/85846475/205490123-91cebb09-6c5f-4b9f-9e2f-26e9f1ebce1f.png" width="350" height="230"><br>
몬스터가 추격 끝에 플레이어 오브젝트를 공격하면 게임 오버된다.</h6>
</div>
<br><br>

##### 6. 멀티 플레이
<div>
<h6>sweeper3D는 멀티 플레이로 진행 가능한 게임이다.<br>
게임 내의 플레이어들의 이동과 몬스터는 PhotonView로, 문에 대한 상호작용 내용은 Photon_RPC로 동기화된다.<br>
게임 스테이지의 설정과 몬스터 생성은 마스터 클라이언트에서 처리한다.</h6>
<h6> 다음은 동기화에 대한 모습이다.<br><br>
<table>
<th colspan="3" align="center">스테이지 현황</th>
<tr><td> </td><td align="center">전</td><td align="center">후</td></tr>
<tr><td>1P</td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205490964-6063ad9d-06b0-41c8-8766-74f461ea0a7e.png" width="250" height="150"></td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491005-033a819f-f21a-4196-8726-001c2df1b478.png" width="250" height="150"></td></tr>
<tr><td>2P</td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491092-39288bab-3d07-468a-83e8-62ec3fe55615.png" width="250" height="150"></td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491110-be29ef2b-80ac-4cc0-9e11-fc0a2a11259f.png" width="250" height="150"></td></tr>
</table><br>

<table>
<th colspan="3" align="center">게임 결과</th>
<tr><td> </td><td align="center">게임 클리어</td><td align="center">게임 오버</td></tr>
<tr><td>1P</td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491349-ea3d9eba-b721-4303-929b-8baeb66bd444.png" width="250" height="150"></td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491384-3ec4dd86-568c-4cde-a615-9e5a54d4b990.png" width="250" height="150"></td></tr>
<tr><td>2P</td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491366-35772f05-5aec-44ba-bbc9-ae8d9bd10582.png" width="250" height="150"></td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491401-9da4e801-db02-4007-9c7e-b6e96905cb10.png" width="250" height="150"></td></tr>
</table><br>

<table>
<th colspan="3" align="center">몬스터 동기화</th>
<tr><td> </td><td align="center">전</td><td align="center">후</td></tr>
<tr><td>1P</td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491585-f9a2a0da-c3bc-4338-b25b-11128fe7bce0.png" width="250" height="150"></td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491600-22678e66-9b68-4789-95fd-4dcd12f6be88.png" width="250" height="150"></td></tr>
<tr><td>2P</td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491618-d871898e-a590-49cc-88bd-dcdee95afc22.png" width="250" height="150"></td>
    <td><img src="https://user-images.githubusercontent.com/85846475/205491629-a083a3ee-80f0-4e71-98ef-e07e28c50ab4.png" width="250" height="150"></td></tr>
</table>
지뢰가 있는 방을 열면 다같이 게임 오버되지만, 몬스터는 개인으로 게임 오버가 적용된다.<br>
게임 진행이 가능한 플레이어가 남지 않았을 때(마지막 플레이어가 게임 오버되었을 때), 대기 중인 모든 플레이어에게 게임 오버 안내가 실행된다.<br>
게임 스테이지를 제어하는 마스터 클라이언트를 제외한 플레이어들은 esc 키를 이용하여 게임 중간에 나갈 수 있다.<br>
마스터 클라이언트도 스테이지에 남은 인원이 본인 혼자밖에 없을 때는 해당 기능을 이용 가능하다.
<br>

</h6>

</div>
<br><br>

***
### 주요 코드
<h6><ul>
<li><a href=https://github.com/SE0NA/sweeper-3D/blob/main/minsweeper/Assets/Scripts/GameManager.cs>GameManager.cs</a></li>
<li><a href="https://github.com/SE0NA/sweeper-3D/blob/main/minsweeper/Assets/Scripts/PlayerController.cs">PlayerController.cs</a></li>
<li><a href="https://github.com/SE0NA/sweeper-3D/blob/main/minsweeper/Assets/Scripts/Enemy.cs">Enemy.cs</a></li>
<li><a href="https://github.com/SE0NA/sweeper-3D/blob/main/minsweeper/Assets/Scripts/EnemySight.cs">EnemySight.cs</a></li>
<li><a href="https://github.com/SE0NA/sweeper-3D/blob/main/minsweeper/Assets/Scripts/Room.cs">Room.cs</a></li>
</ul></h6>

***
#### 사용 에셋
<h6><table>
<tr><td>플레이어 오브젝트</td><td>Robot Solider - Marcelo Barrio</td></tr>
<tr><td>스테이지 구성</td><td>Sci-fi Styled Modular Pack - karboosx</td></tr>
<tr><td>몬스터 오브젝트</td><td>Meshtint Free Polygonal Metalon – Meshtint Studio</td></tr>
<tr><td>음향 효과</td><td><p>Free Sound Effects Pack – Olivier Girardot <br>Ash Valley Cybernetics LITE – Neal Bond <br>Horror Elements – Anthon</p></td></tr>
<tr><td>애니메이션</td><td><p>Simple FX – Cartoon Particles – Synty Studios <br>Basic Motions FREE – Kevin Iglesias</p></td></tr>
</table></h6>
