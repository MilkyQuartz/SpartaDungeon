# 모험가 파티의 이야기: 수정, 태일, 인수, 성훈의 모험 

## Scenario #1: 그들의 첫만남 (개발자소개)

- ##### 유수정
  - ###### 어머니께서 해주시는 옛 전설 속의 이야기를 통해 작은 글만으로도 세상을 변화시키는 코딩 마법사의 힘과 지혜를 동경하며 자랐지만, 시간이 흐를수록 꿈은 잊게되고 소중하게 생각하던 작은 글마저 잊게되었습니다. 그러나 그녀의 마음 깊은 곳에는 어릴 적 소중하게 여기던 것을 잃게 만든 존재를 찾는 욕구를 찾기위해 모험을 떠나는데...!
- ##### 강태일
  - ###### 전쟁터에서 능숙한 프로그래밍 실력으로 명성을 떨쳤습니다. 하지만 전투의 강렬함과 산전수전을 겪으면서 얻은 깊은 상처와 피로로 인해 코딩에 대한 의욕을 잃고 있었습니다. 그러던 어느 날, 유수정과 우연한 만남을 통해 자신의 목표를 다시 찾게 되었고, 그녀의 지혜와 함께 새로운 모험에 동참하기로 결심했습니다.
- ##### 박인수
  - ###### 코드와 코드의 조화를 통해 세상을 지키는 것을 사명으로 삼았습니다. 어릴 적부터 부모님으로부터 코딩술을 배우며 성장한 그는 책상 앞에서 산다는 것이 자신의 운명임을 깨달았습니다. 그의 코드는 정확하고 빠르며, 그는 코드와의 소통을 통해 전쟁의 무서움을 넘어 새로운 미래를 찾고자 했습니다. 그리고 아직도 찾고자 하성훈의 뒤를 쫒습니다.
- ##### 하성훈
  - ###### 어려운 환경에서 자란 해커였습니다. 그는 어릴 적부터 생존을 위해 필요한 컴퓨터 기술을 익히며 살아왔으며, 그의 손가짐과 코드의 재치는 전쟁터 속에서도 빛을 발했습니다. 하지만 그의 내면에는 과거의 상처와 자신을 둘러싼 의문이 남아있었습니다. 한참 책상 앞을 헤매다 좌절하지만 다른 개발자들과 함께하는 모험을 통해 자신의 과거를 극복하고자하는데...!
##### 이렇게 서로 다른 과거와 능력을 지닌 네 사람이 완벽한 코드 실력자가 되기위해 모인 후, 하나의 목표를 향해 모험을 떠나게 되었습니다. 함께하면서 서로를 이해하고 믿음을 쌓아나가며, 그들은 더 성장해 나갈 것입니다...!! 이들의 성장 이야기를 기대해주세요!

## Scenario #2: 주요 기능
### 0. 로그인
- 플레이어 정보(Name)이 ID로 ID별로 플레이어 능력치와 인벤토리가 구분되어 저장된다.
- 이미 있는 플레이어로 새로 시작하려고 하면 거절되고 종료된다.
- 저장되어있지 않은 플레이어로 불러오기를 하려고하면 거절되고 종료된다.
- 저장되어있는 정보로 불러오기시 기존 플레이어 정보와 인벤토리를 불러온다.
- 새로 시작할때 직업별 능력치가 달라 직업별로 능력치가 다르게 저장된다. 

### 1. 상태보기
- 캐릭터의 정보가 표기된다. (아이템 착용시 +되어 표기됨.)
  
### 2. 인벤토리
- 보유중인 아이템을 관리할 수 있다.
- 아이템 목록을 확인할 수 있고 비어있으면 인벤토리가 비어있습니다를 표기한다.
- 장착하거나 장착 해제가 가능하다.
- 소비아이템은 장착이 불가능하다고 경고하며 선택할 수 있는 창으로 돌아간다.

### 3. 상    점
- 아이템을 구매하거나 판매할 수 있는 시스템이다.
- 이미 구매한 아이템은 구매가 불가능하다.
- 판매 후에는 재구매가 가능하다.
  
### 4. 던    전
- 던전에 들어가 골드를 벌 수 있다.
- 난이도별로 레벨과 타입이 다른 몬스터들이 나오게된다.
- 체력이 0이하임에도 도전하려고하면 경고문구가 나오며 메인화면으로 돌아가게 된다.
- 도망가면 최종보상을 받을 수 없다.

### 5. 주    점
- Hp와 Mp를 올리기위한 휴식 공간이다.
- Max보다 더 높게 올릴 수 없다.
- Gold가 부족할 경우 구매할 수 없다.
- 테이크아웃 기능으로 인벤토리에 담아서 소비아이템으로 사용할 수 있다.
  
### 6. 길    드
- 퀘스트를 받고, 깨고, 보상을 받을 수 있는 시스템이다.
- 목표치에 도달시 클리어 처리가 되고 받을 수 있다.

### 7. $카지노$
- 현재 소지하고 있는 Gold로 배팅을 하여 돈을 벌거나 잃을 수 있는 시스템이다.
- Gold가 부족할 시 도전할 수 없다.

### 8. 게임종료
- 게임종료시 기존의 정보를 모두 저장하고 종료를 하게된다.

## Scenario #3: 실행 화면
- 이미지 클릭시 구현 영상으로 이동합니다.
  
[![Main](https://github.com/MilkyQuartz/SpartaDungeon/assets/141620531/4af158f0-a30d-41b6-9f29-664820201459)
](https://youtu.be/Bsqa1bmoJBY)
