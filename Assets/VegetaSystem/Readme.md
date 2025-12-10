Đối với UI : ---------------------------------------------------------------

screen kế thừa từ BaseScreen
popup kế thừa từ BasePopup
notify kế thừa từ BaseNotify
overlap kế thừa từ BaseOverlap

tạo thêm 1 UIManager để kế thừa UIManagerSystem

Cách gọi của UI sẽ tương tự như nhau đối với các loại khác nhau chỉ khác hậu tố :

Ví dụ với Screen : (Đối với các loại khác đổi tên hậu tố là được)

UIManager.Instance.GetScreen<typeScreenName>();  // get screen chỉ định , nếu chưa kéo vào scene sẽ tự động instantiate từ prefab trong source, sau đó trả về screen này
UIManager.Instance.ShowScreen<typeScreenName>();  // hiện screen được chỉ định và trả về screen này , nếu screen chưa được kéo vào scene sẽ tự động instantiate từ prefab trong source
UIManager.Instance.HideScreen<typeScreenName>();  // ẩn screen được chỉ định 
UIManager.Instance.HideAllScreen();  // Ân tất cả screen 
UIManager.Instance.IsScreenActive<typeScreenName>();  // check xem screen này có đang hiện không ( nếu không hiện hoặc chưa có trên scene sẽ trả về false)  -- ít dùng
UIManager.Instance.HasScreenActiveExcept<typeScreenName>();  // check xem có scene nào đang hiện ngoại trừ scene được chỉ định hay không - ít dùng


+ Setup scene : Vào mục VegetaSystem -> UI , Kéo phần UIManager prefab vào trong scene và Unpack complete nó đi , khi tạo 1 screen kéo vào các folder chỉ định sẵn 
ví dụ cScreen sẽ chứa các screen . Đối với những bạn không muốn kéo thẳng vào scene mà muốn để dạng prefab , đầu tiên cần kéo thư mục Resouces vào trong dự án của các bạn , 
không kéo thì cũng không vấn đề gì nhưng để clean nhất thì nên kéo vào dự án các bạn. Đường dẫn như sau  Resoucres -> Prefabs -> UI -> (cScreen , cPopup, cNotify, cOverlap), kéo các prefab các loại vào container 
tựa như kéo vào các container trên scene , screen vào cScreen , popup vào cPopup ,...

+ Note : UI khi chạy các hàm show hoặc get nếu không có trên scene sẽ tự động vào mục prefab để lấy và khởi tạo , ui khi ẩn đi sẽ object sẽ không bị ẩn (tránh trường hợp gây các lỗi bởi ẩn lập tức) mà nó sẽ set alpha 
của canvasgroup = 0 và đó blockraycast = false


Đối với Pool : --------------------------------------------------------------------

Những object tạo nhiều và tái sử dụng nhiều lần nên dùng pool , có 2 dạng object pool , 1 dạng có 1 biến thể , và 1 dạng có nhiều biến thể 

Ví dụ : viên đạn chỉ có 1 loại (dạng 1 biến thể ), bong bóng ( có nhiều màu -> nhiều loại biến thể )

+ Setup : 

Nên tạo 1 class PoolManager kế thừa PoolManagerSystem ( mục đích tránh sửa chữa trong các class system ), tạo 1 object PoolManager và kéo script này vào

Tạo các scriptable object cho object Pool : Create -> Pool -> PoolData

Trong scriptable object config như sau :

Đối với các object có 1 biến thể : 

Mục Head (Config with simple prefab) -> prefab (kéo vào đây)
InitAmount : Số lượng mặc định khởi tạo (nếu set 0  thì khi nào get nó tự động khởi tạo).

Đối với các object có nhiều biến thể :

Tích vào mục : IsMutiple = true
Mục Head (Config with mutiple prefabs) , chỗ List Pool Items , kéo các biến thể vào cũng như Init Amount của riêng từng biến thể (nếu set 0  thì khi nào get nó tự động khởi tạo).

Sau đó kéo scriptable object này vào list trong object poolmanager

+ Cách implement code:

Đối với dạng 1 biến thể xem ví dụ mục Sample_Cube (trong phần sample)

Đối với dạng nhiều biến thể xem ví dụ mục Sample_Sphere (trong phần sample) :

Dạng nhiều biến thể cần string để nhận biết các biến thể đó ra sao , như prefab nào là màu đó , prefab nào là blue , vì truyền vào string thì quá thụ động nên khuyến cáo prefab này có 
enum thì hay rồi chuyển dạng enum đó thành string 

Trong ví dụ của Sample_Sphere có enum là Sample_SphereType (Red và Blue) , dạng nhiều biến thể sẽ phải kế thừa từ IMultiPoolable , implement hàm GetSubKeyPool, rồi ta triển khai như trong sample là được , 
mục đích như vậy để pool có thể nhận biết được các biến thể khác nhau dù cùng type


Cách gọi lấy 1 object pool :

Đối với object 1 biến thể : PoolManager.Instance.GetObj<TypeObj>();
Đối với object nhiều biến thể : PoolManager.Instance.GetObj<TypeObj>(enumtype.tostring()); , object nhiều biến thể ta cần truyền vào string để nhận biết biến thể nào , khuyến cáo dùng enum tostring 
của các biến thể 

Cách release :
PoolManager.Instance.Release(object instance);  khi release cần truyền vào instance cần release , (object có sẵn biến isrelease mọi ng có thể set = false trong hàm get và = true trong hàm release để check object đó đã
release hay chưa), hàm release còn có thêm 2 param ignoreParentPool (mặc định = false)  khi release sẽ tự động setparent vào container trong pool nếu set bằng true, worldPosStay mặc định = true ( dành cho các prefab ui
nên các bạn không cần quan tâm lắm), tóm lại 2 param phía sau không cần quan tâm nhiều  vì cũng ít dùng

+ Note : Nếu object 1 biến thể gọi hàm có param của object nhiều biến thể trình biên dịch sẽ báo lỗi luôn , còn ngược lại nếu object nhiều biến thể gọi hàm 1 biến thể (tức không truyền param) tuy trình biên dịch không báo lỗi nhưng trong lúc run sẽ throw exception


Đối với LoadScene : --------------------------------------------------------------------
Cũng nên tạo 1 class LoadSceneManager kế thừa LoadSceneSystem

Xem trong class Sample_MenuScreen -> method OnOnClickNextBtn , hoặc Sample_GamePlaySettingPopup -> method OnClickHomeBtn

Đầu tiên ta tạo ConfigLoadScene

sceneName là bắt buộc còn tất cả đều là option ( nếu không truyền cũng không sao)

Quy trình chạy của config , sẽ chạy hàm OnBeforeLoad (nếu khác null) đầu tiên ,  sau đó sẽ chờ DelayBeforeLoad  nếu lớn hơn 0 , sau khi chờ sẽ tiến hành loadscene , 
trong lúc load scene sẽ gọi hàm OnProgress để lấy quá trình load , và kết hợp gọi OnLoadApi (nếu khác null) nếu ta truyền vào hàm này , khi quá trình loadscene xong , hoặc đối với có loadapi nó sẽ chờ cả 2 quá trình load này xong hết
rồi tiếp tục chạy , chờ  DelayCompleted nếu lớn hơn 0 , và sẽ gọi OnAfterLoad (nếu khác null), đối với IgnoreDisplayProgress biến này dành cho fake quá trình chạy , vì nhiều lúc scene load quá nhanh trong 1 ticktack
nên quá trình chạy nếu có thanh process loading nó sẽ giật 1 phát lên 100 ngay , nó rất kì  để tránh trường hợp này và ta muốn thanh process chạy từ từ lên 100 IgnoreDisplayProgress sẽ mặc định là false, 
trường hợp các bạn muốn set bằng true cũng được dành cho trường hợp muốn đổi scene ngay tức khắc mà không có màn hình loading hay gì đó.

sau khi config xong ConfigLoadScene
chạy LoadSceneManager.Instance.LoadNewScene (config) // hàm này sẽ có thêm 1 param tùy chọn là force (mặc định sẽ là false), dùng force khi bán muốn load lại scene hiện tại , còn nếu force = false , nó sẽ báo lỗi khi 
scene hiện tại đang active mà bạn vẫn cố gắng load



Lưu ý :-------------------------------------------------------

Phần load scene dùng Unitask không khuyến cáo các bạn cũng dùng theo khi chưa biết cách dùng (vì có những lệnh unitask sẽ chạy đa luồng lúc đó nếu không 
biết kiểm soát sẽ gây crash).





















